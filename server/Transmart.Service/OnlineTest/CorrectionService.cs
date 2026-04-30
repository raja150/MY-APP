using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Domain.Models.OnlineTest.Request;

namespace TranSmart.Service.OnlineTest
{
	public interface ICorrectionService : IBaseService<Result>
	{
		Task<IEnumerable<CorrectionList>> CorrectionTestList(Guid empId);
		Task<IEnumerable<CorrectionModel>> GetQuestions(Guid paperId);
		Task<Result<ResultQuestion>> ManualCorrection(List<VerifyAnswerRequest> request, Guid paperId);
		Task<IEnumerable<ResultQuestion>> GetAnswers(Guid questionId);
	}
	public class CorrectionService : BaseService<Result>, ICorrectionService
	{
		public CorrectionService(IUnitOfWork uow) : base(uow)
		{

		}

		public async Task<IEnumerable<CorrectionList>> CorrectionTestList(Guid empId)
		{
			var correctionList = new List<CorrectionList>();

			var resultQuestion = await UOW.GetRepositoryAsync<ResultQuestion>()
							.GetAsync(x => x.Paper.OrganiserId == empId && !x.ManuallyCorrected
								&& !x.IsCorrect && !string.IsNullOrEmpty(x.Answer)
								&& (x.Question.Type == (int)QuestionType.TextBox || x.Question.Type == (int)QuestionType.Numeric),
								include: x => x.Include(x => x.Paper),
								orderBy: o => o.OrderByDescending(o => o.AddedAt));

			foreach (var result in resultQuestion.DistinctBy(x => x.PaperId))
			{
				correctionList.Add(new CorrectionList
				{
					Test = result.Paper.Name,
					TotalAttendees = resultQuestion.Where(x => x.PaperId == result.PaperId).DistinctBy(x => x.EmployeeId).Count(),
					ID = result.PaperId,
					TestOn = result.AddedAt.Date
				});
			}
			return correctionList;
		}

		public async Task<Result<ResultQuestion>> ManualCorrection(List<VerifyAnswerRequest> request, Guid paperId)
		{
			var result = new Result<ResultQuestion>();

			try
			{
				foreach (var item in request)
				{
					var td = await UOW.GetRepositoryAsync<ResultQuestion>().SingleAsync(x => x.ID == item.ID);
					if (td != null)
					{
						td.IsCorrect = item.IsCorrect;
						td.ManuallyCorrected = true;
						UOW.GetRepositoryAsync<ResultQuestion>().UpdateAsync(td);
					}
				}

				var empResults = await UOW.GetRepositoryAsync<Result>().GetAsync(x => x.PaperId == paperId);
				foreach (var item in empResults)
				{
					//Calculating total marks and percentage
					item.TotalMarks = request.Count(x => x.EmployeeId == item.EmployeeId && x.IsCorrect) +
						empResults.FirstOrDefault(x => x.EmployeeId == item.EmployeeId).TotalMarks;
					item.Percentage = DecimalUtil.Percentage(item.TotalMarks, empResults.FirstOrDefault(x => x.EmployeeId == item.EmployeeId).TotalQuestions);
					UOW.GetRepositoryAsync<Result>().UpdateAsync(item);
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

		public async Task<IEnumerable<ResultQuestion>> GetAnswers(Guid questionId)
		{
			return await UOW.GetRepositoryAsync<ResultQuestion>()
				.GetAsync(x => x.QuestionId == questionId && !x.ManuallyCorrected && !string.IsNullOrEmpty(x.Answer) && !x.IsCorrect); ;
		}

		public async Task<IEnumerable<CorrectionModel>> GetQuestions(Guid paperId)
		{
			var correctionList = new List<CorrectionModel>();

			var resultQuestion = await UOW.GetRepositoryAsync<ResultQuestion>()
				.GetAsync(x => x.PaperId == paperId && !x.IsCorrect && !x.ManuallyCorrected && !string.IsNullOrEmpty(x.Answer)
								&& (x.Question.Type == (int)QuestionType.TextBox || x.Question.Type == (int)QuestionType.Numeric),
						include: x => x.Include(x => x.Question));

			var queAns = await UOW.GetRepositoryAsync<QuestionAnswer>().GetAsync(x => x.Question.PaperId == paperId);

			foreach (var resQue in resultQuestion.DistinctBy(x => x.QuestionId))
			{
				var item = new CorrectionModel
				{
					Question = resQue.Question.Text,
					ID = resQue.QuestionId,
					CorrectAns = queAns.First(x => x.QuestionId == resQue.QuestionId).AnswerTxt
				};
				correctionList.Add(item);
			}

			return correctionList;
		}
	}
}
