using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Data.Repository.OnlineTest;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;

namespace TranSmart.Service.SelfService.OnlineTest
{
	public interface ITestService : IBaseService<ResultQuestion>
	{
		Task<Result<ResultQuestion>> SaveAnswer(List<ResultQuestion> model, Guid paperId, bool IsFinish, Guid empId);
		Task<Result<IEnumerable<ResultQuestion>>> Start(Guid paperId, Guid empId);
		Task<IEnumerable<dynamic>> TestList(Guid empId, Guid deptId, Guid desgId);
		Task<bool> IsAllowed(Guid empId, Guid paperId);
	}
	public class TestService : BaseService<ResultQuestion>, ITestService
	{
		private readonly ITestRepository _repository;
		public TestService(IUnitOfWork uow, ITestRepository repository) : base(uow)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<dynamic>> TestList(Guid empId, Guid deptId, Guid desgId)
		{
			return await _repository.Test(empId, deptId, desgId);
		}

		private async Task<Result<IEnumerable<ResultQuestion>>> DisableReTake(Guid paperId, Guid empId, Result<IEnumerable<ResultQuestion>> result)
		{
			try
			{
				var resQues = await UOW.GetRepositoryAsync<ResultQuestion>().GetAsync(x => x.PaperId == paperId && x.EmployeeId == empId);
				foreach (var item in resQues.Where(x => x.ReTake))
				{
					item.ReTake = false;
					UOW.GetRepositoryAsync<ResultQuestion>().UpdateAsync(item);
				}
				await UOW.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public async Task<Result<IEnumerable<ResultQuestion>>> Start(Guid paperId, Guid empId)
		{
			var result = new Result<IEnumerable<ResultQuestion>>();

			var ResultQuestion = await GetTestQue(paperId, empId);
			if (ResultQuestion.Any())
			{
				var response = await DisableReTake(paperId, empId, result);
				if (response.HasError)
				{
					return result;
				}
				result.ReturnValue = ResultQuestion;
				return result;
			}
			try
			{
				bool isJumbled = false;
				var testEmployee = await UOW.GetRepositoryAsync<TestEmployee>().SingleAsync(x => x.PaperId == paperId && !x.IsDelete,
									include: x => x.Include(x => x.Paper));
				if (testEmployee != null) { isJumbled = testEmployee.Paper.IsJumbled; }

				var testDept = await UOW.GetRepositoryAsync<TestDepartment>().SingleAsync(x => x.PaperId == paperId && !x.IsDelete,
									include: x => x.Include(x => x.Paper));
				if (testDept != null) { isJumbled = testDept.Paper.IsJumbled; }

				var testDesg = await UOW.GetRepositoryAsync<TestDesignation>().SingleAsync(x => x.PaperId == paperId && !x.IsDelete,
									include: x => x.Include(x => x.Paper));
				if (testDesg != null) { isJumbled = testDesg.Paper.IsJumbled; }

				var questions = await UOW.GetRepositoryAsync<Question>().GetAsync(x => x.PaperId == paperId && !x.IsDelete);
				if (isJumbled)
				{
					//Jumbling the questions
					Random rnd = new();
					questions = questions.OrderBy(x => rnd.Next());
				}

				foreach (var question in questions)
				{
					await UOW.GetRepositoryAsync<ResultQuestion>().AddAsync(new ResultQuestion
					{
						QuestionId = question.ID,
						PaperId = paperId,
						TestEmployeeId = testEmployee?.ID,
						TestDepartmentId = testDept?.ID,
						TestDesignationId = testDesg?.ID,
						EmployeeId = empId
					});
				}
				await UOW.SaveChangesAsync();
				result.ReturnValue = await GetTestQue(paperId, empId);
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		private async Task<IEnumerable<ResultQuestion>> GetTestQue(Guid paperId, Guid empId)
		{
			return await UOW.GetRepositoryAsync<ResultQuestion>().GetAsync(x => x.PaperId == paperId && x.EmployeeId == empId,
				include: x => x.Include(x => x.Question).ThenInclude(x => x.Choices)
				.Include(x => x.Paper));
		}

		public async Task<Result<ResultQuestion>> SaveAnswer(List<ResultQuestion> model, Guid paperId, bool IsFinish, Guid empId)
		{
			var result = new Result<ResultQuestion>();
			try
			{
				int timeSpent = 0; int totalMarks = 0;
				foreach (var item in model)
				{
					if (!string.IsNullOrEmpty(item.Answer) || item.TimeSpent != 0)
					{
						var ResultQuestion = await UOW.GetRepositoryAsync<ResultQuestion>().SingleAsync(x => x.ID == item.ID);
						ResultQuestion.Answer = item.Answer;
						ResultQuestion.TimeSpent = item.TimeSpent;
						ResultQuestion.IsCorrect = await ValidateAns(item, ResultQuestion.QuestionId, result);
						if (IsFinish)
						{
							timeSpent += item.TimeSpent;
							totalMarks = ResultQuestion.IsCorrect ? ++totalMarks : totalMarks;
						}
						UOW.GetRepositoryAsync<ResultQuestion>().UpdateAsync(ResultQuestion);
					}
				}
				if (IsFinish)
				{
					// Adding final data to the EmpTest table
					result = await AddResult(model, result, paperId, timeSpent, totalMarks, empId);
					if (result.HasError)
					{
						return result;
					}
				}
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}

			return result;
		}

		private async Task<bool> ValidateAns(ResultQuestion model, Guid questionId, Result<ResultQuestion> result)
		{
			bool isCorrect = false;
			try
			{
				if (!string.IsNullOrEmpty(model.Answer))
				{
					var question = await UOW.GetRepositoryAsync<Question>().SingleAsync(x => x.ID == questionId);
					if (question.Type == (int)QuestionType.Single)
					{
						isCorrect = await UOW.GetRepositoryAsync<QuestionAnswer>().HasRecordsAsync(x => x.ChoiceId == Guid.Parse(model.Answer));
					}
					else if (question.Type == (int)QuestionType.Multiple)
					{
						string[] multiCorrect = model.Answer.Split(",");
						var correctAns = await UOW.GetRepositoryAsync<QuestionAnswer>().GetAsync(x => x.QuestionId == questionId);
						if (correctAns.Count() == multiCorrect.Length)
						{
							int count = 0;
							foreach (var correct in multiCorrect)
							{
								if (correctAns.SingleOrDefault(x => x.ChoiceId == Guid.Parse(correct)) != null)
								{
									count++;
								}
							}
							isCorrect = correctAns.Count() == count;
						}
						else
						{
							isCorrect = false;
						}
					}
					else
					{
						isCorrect = await UOW.GetRepositoryAsync<QuestionAnswer>()
							.HasRecordsAsync(x => x.QuestionId == questionId && x.AnswerTxt.ToLower().Equals(model.Answer.Trim().ToLower()));
					}
				}

			}
			catch (Exception ex)
			{
				return result.AddMessageItem(new MessageItem(ex));
			}

			return isCorrect;
		}

		private async Task<Result<ResultQuestion>> AddResult(List<ResultQuestion> ResultQuestion,
			 Result<ResultQuestion> result, Guid paperId, int timeSpent, int totalMarks, Guid empId)
		{
			try
			{
				var empTest = await UOW.GetRepositoryAsync<Result>().SingleAsync(x => x.PaperId == paperId && x.EmployeeId == empId);
				if (empTest == null)
				{
					await UOW.GetRepositoryAsync<Result>().AddAsync(new Result
					{
						TotalQuestions = ResultQuestion.Count,
						EmployeeId = empId,
						PaperId = paperId,
						TotalMarks = totalMarks,
						Wrong = ResultQuestion.Count - totalMarks,
						TotalTime = timeSpent,
						Percentage = DecimalUtil.Percentage(totalMarks, ResultQuestion.Count)
					});
				}
				else
				{
					empTest.TotalQuestions = ResultQuestion.Count;
					empTest.TotalMarks = totalMarks;
					empTest.Wrong = ResultQuestion.Count - totalMarks;
					empTest.TotalTime = timeSpent;
					empTest.Percentage = DecimalUtil.Percentage(totalMarks, ResultQuestion.Count);
					UOW.GetRepositoryAsync<Result>().UpdateAsync(empTest);
				}
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex.Message));
			}
			return result;
		}

		public async Task<bool> IsAllowed(Guid empId, Guid paperId)
		{
			var count = await UOW.GetRepositoryAsync<ResultQuestion>()
				.SumOfIntAsync(x => x.EmployeeId == empId && x.PaperId == paperId && !x.ReTake, x => x.TimeSpent);

			return count == 0;
		}
	}
}
