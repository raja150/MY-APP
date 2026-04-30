using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.Response;

namespace TranSmart.Service.OnlineTest
{
	public interface IResultService : IBaseService<Result>
	{
		Task<ResultSummaryModel> GetSummary(Guid resultId);
		Task<IPaginate<Result>> EmpPaginate(ResultSearch search);
		Task<IEnumerable<Result>> GetPapers(Guid empId);
		Task<Result<ResultQuestion>> AllowReTake(Guid paperId, Guid empId);


	}
	public class ResultService : BaseService<Result>, IResultService
	{
		public ResultService(IUnitOfWork uow) : base(uow)
		{

		}

		#region filter dropdown

		//For empScreen
		public async Task<IEnumerable<Result>> GetPapers(Guid empId)
		{
			return await UOW.GetRepositoryAsync<Result>().GetAsync(predicate: x => x.EmployeeId == empId, include: x => x.Include(x => x.Paper));
		}
		#endregion


		#region Paginate
		// for Admin 
		public override async Task<IPaginate<Result>> GetPaginate(BaseSearch baseSearch)
		{
			var search = (ResultSearch)baseSearch;
			return await UOW.GetRepositoryAsync<Result>()
				.GetPageListAsync(predicate: x => (x.EmployeeId == search.EmployeeId || !search.EmployeeId.HasValue) &&
				(x.PaperId == search.PaperId || !search.PaperId.HasValue) &&
				(!search.TestDate.HasValue || search.TestDate.Value.Date == x.AddedAt.Date),
				include: i => i.Include(x => x.Paper).Include(x => x.Employee),
				index: search.Page, size: search.Size, sortBy: search.SortBy ?? "AddedAt", ascending: search.IsDescend);
		}

		public async Task<IPaginate<Result>> EmpPaginate(ResultSearch search)
		{

			return await UOW.GetRepositoryAsync<Result>()
				.GetPageListAsync(predicate: x => x.EmployeeId == search.RefId &&
				(!search.PaperId.HasValue || x.PaperId == search.PaperId),
				include: i => i.Include(x => x.Paper).Include(x => x.Employee),
				index: search.Page, size: search.Size, sortBy: search.SortBy ?? "AddedAt", ascending: search.IsDescend);
		}
		#endregion

		public async Task<ResultSummaryModel> GetSummary(Guid resultId)
		{
			var result = await UOW.GetRepositoryAsync<Result>().SingleAsync(x => x.ID == resultId,
																include: x => x.Include(x => x.Paper).Include(x => x.Employee));
			if (result != null)
			{
				var resultQue = await UOW.GetRepositoryAsync<ResultQuestion>()
					.GetAsync(x => x.PaperId == result.PaperId && x.EmployeeId == result.EmployeeId,
					include: x => x.Include(x => x.Question).ThenInclude(x => x.Choices));

				var UnAnswered = resultQue.Count(x => string.IsNullOrEmpty(x.Answer));

				var resultview = new ResultSummaryModel
				{
					TotalQuestion = result.TotalQuestions,
					Percentage = result.Percentage,
					Correct = result.TotalMarks,
					Wrong = result.TotalQuestions - result.TotalMarks,
					UnAnswered = UnAnswered,
					Name = result.Paper.Name,
					TimeSpent = result.TotalTime,
					TotalTime = result.Paper.Duration,
					Answered = result.TotalQuestions - UnAnswered,
					PaperId = result.PaperId,
					EmpId = result.EmployeeId,
					EmpName = result.Employee.Name,
					QueAnswerModel = await GetReview(resultQue)
				};
				return resultview;
			}
			return null;
		}


		private async Task<IEnumerable<QuestionAnswerModel>> GetReview(IEnumerable<ResultQuestion> resultQue)
		{
			var QAModel = new List<QuestionAnswerModel>();

			foreach (var item in resultQue)
			{
				var testQA = new QuestionAnswerModel();

				var correctAns = await UOW.GetRepositoryAsync<QuestionAnswer>()
						.GetAsync(x => x.QuestionId == item.QuestionId, include: x => x.Include(x => x.Choice));

				if (item.Question.Type == (int)QuestionType.Single)
				{
					testQA.UserAnswer = string.IsNullOrEmpty(item.Answer) ? null : item.Question.Choices.FirstOrDefault(x => x.ID == Guid.Parse(item.Answer))?.Text;
					testQA.CorrectAnswer = correctAns.FirstOrDefault()?.Choice?.Text;
				}
				else if (item.Question.Type == (int)QuestionType.Multiple)
				{
					foreach (var crct in correctAns.ToList())
					{
						testQA.CorrectAnswer += crct.Choice.Text + ",";
					}

					if (string.IsNullOrEmpty(item.Answer))
					{
						testQA.UserAnswer = null;
					}
					else
					{
						var mutliAns = item.Answer.Split(",");
						foreach (var ans in mutliAns)
						{
							testQA.UserAnswer += item.Question.Choices.FirstOrDefault(x => x.ID == Guid.Parse(ans))?.Text + ",";
						}
					}
				}
				else
				{
					testQA.UserAnswer = item.Answer;
					testQA.CorrectAnswer = correctAns.FirstOrDefault(x => x.QuestionId == item.QuestionId).AnswerTxt;
				}
				testQA.Type = item.Question.Type;
				testQA.Text = item.Question.Text;
				QAModel.Add(testQA);
			}
			return QAModel;
		}

		public async Task<Result<ResultQuestion>> AllowReTake(Guid paperId, Guid empId)
		{
			var result = new Result<ResultQuestion>();

			try
			{
				var resultQues = await UOW.GetRepositoryAsync<ResultQuestion>().GetAsync(x => x.PaperId == paperId && x.EmployeeId == empId);
				if (resultQues.Any())
				{
					foreach (var item in resultQues)
					{
						item.ReTake = true;
						UOW.GetRepositoryAsync<ResultQuestion>().UpdateAsync(item);
					}
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
				}
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}
	}
}
