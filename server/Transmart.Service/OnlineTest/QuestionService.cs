using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
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
	public interface IQuestionService : IBaseService<Question>
	{
		Task<Result<Question>> Add(Question entityModel, QuestionModel model, Guid testId);
		Task<Result<Question>> Delete(Guid id);
		Task<Result<Question>> Update(Question item, QuestionModel model);
		Task<QuestionModel> GetQueById(Guid queId);
		Task<Result<Question>> AddBulk(List<Question> entityData, List<QuestionModel> items);
		Task<IEnumerable<QuestionAnswer>> GetQuestionAnswers(Guid paperId);


	}
	public class QuestionService : BaseService<Question>, IQuestionService
	{
		public QuestionService(IUnitOfWork uow, ISequenceNoService sequenceNoService) : base(uow, sequenceNoService)
		{

		}

		public async Task<IEnumerable<QuestionAnswer>> GetQuestionAnswers(Guid paperId)
		{
			return await UOW.GetRepositoryAsync<QuestionAnswer>().GetAsync(x => x.Question.PaperId == paperId && !x.Question.IsDelete,
				include: x => x.Include(x => x.Choice)
				.Include(x => x.Question).ThenInclude(x => x.Paper)
				.Include(x => x.Question).ThenInclude(x => x.Choices));
		}

		public async Task<QuestionModel> GetQueById(Guid queId)
		{
			var question = new QuestionModel();

			var QueAns = await UOW.GetRepositoryAsync<QuestionAnswer>().GetAsync(x => x.QuestionId == queId,
				include: x => x.Include(x => x.Question).ThenInclude(x => x.Choices)
				.Include(x => x.Choice));

			//for multiType question will get more than two records so to avoid loop assinging first record as everything will be same except key
			var questionAnswer = QueAns.FirstOrDefault();
			if (questionAnswer != null)
			{
				var options = new List<ChoiceModel>();
				foreach (var option in questionAnswer.Question.Choices.OrderBy(x => x.SNo))
				{
					options.Add(new ChoiceModel
					{
						ID = option.ID,
						Text = option.Text,
						SNo = option.SNo
					});
				}
				question.Text = questionAnswer.Question.Text;
				question.PaperId = questionAnswer.Question.PaperId;
				question.ID = questionAnswer.QuestionId;
				question.Choices = options;
				question.Type = questionAnswer.Question.Type;

				if (questionAnswer.Question.Type == (int)QuestionType.Single)
				{
					question.Key = questionAnswer.Choice.Text;
				}
				else if (questionAnswer.Question.Type == (int)QuestionType.Multiple)
				{
					var key = "";
					foreach (var item in QueAns)
					{
						key += item.Choice.Text + ",";
					}
					question.Key = key.Remove(key.Length - 1);
				}
				else
				{
					question.Key = questionAnswer.AnswerTxt;
				}
			}
			return question;
		}


		public override async Task<IPaginate<Question>> GetPaginate(BaseSearch baseSearch)
		{
			var search = (PaperSearch)baseSearch;
			return await UOW.GetRepositoryAsync<Question>().GetPageListAsync(
				predicate: x => !x.Paper.MoveToLive && !x.IsDelete && x.Paper.OrganiserId == baseSearch.RefId && x.Paper.Status &&
				(!search.PaperId.HasValue || x.PaperId == search.PaperId)
				&& (!search.Type.HasValue || x.Type == search.Type),
				include: i => i.Include(x => x.Paper),
				index: baseSearch.Page,
				size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Paper.Name", ascending: !baseSearch.IsDescend);

		}

		private async Task<Result<Question>> CheckDuplicate(Question item, Result<Question> result)
		{
			if (await UOW.GetRepositoryAsync<Question>()
					.HasRecordsAsync(x => x.Text.Equals(item.Text.Trim()) && x.PaperId == item.PaperId && !x.IsDelete && item.ID != x.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Question_Already_Exists));
			}
			return result;
		}

		public async Task<Result<Question>> Add(Question entityModel, QuestionModel model, Guid testId)
		{
			var result = await AddQuestion(testId, model, entityModel);
			if (result.HasNoError)
			{
				try
				{
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
				}
				catch (Exception ex)
				{
					result.AddMessageItem(new MessageItem(ex));
				}
			}
			return result;
		}

		private async Task<Result<Question>> CheckTestIsLive(Guid paperId, Result<Question> result)
		{
			if (await UOW.GetRepositoryAsync<ResultQuestion>().HasRecordsAsync(x => x.PaperId == paperId))
			{
				result.AddMessageItem(new MessageItem(Resource.Paper_is_in_live));
			}
			return result;
		}

		private async Task<Result<Question>> AddQuestion(Guid paperId, QuestionModel model, Question entityModel)
		{
			var result = new Result<Question>();

			// Checking if the test is in live
			result = await CheckTestIsLive(paperId, result);
			if (result.HasError)
			{
				return result;
			}

			result = await CheckDuplicate(entityModel, result);
			if (result.HasError)
			{
				return result;
			}

			try
			{
				var sNo = (await UOW.GetRepositoryAsync<Question>().GetAsync(x => x.PaperId == paperId)).Count();
				entityModel.SNo = entityModel.SNo == 0 ? ++sNo : entityModel.SNo;

				//Adding data to QuestionAnswer
				await AddOrUpdateQA(model, entityModel);
				await UOW.GetRepositoryAsync<Question>().AddAsync(entityModel);
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}



		public async Task<Result<Question>> Update(Question item, QuestionModel model)
		{
			var result = await UpdateQue(model, item);
			if (result.HasNoError)
			{
				try
				{
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
				}
				catch (Exception ex)
				{
					result.AddMessageItem(new MessageItem(ex));
				}
			}
			return result;
		}

		private async Task<Result<Question>> UpdateQue(QuestionModel model, Question item)
		{
			var result = new Result<Question>();
			var IsSkip = false;//to skip deleting records in questionAnswer entity multiple times.

			var question = await UOW.GetRepositoryAsync<Question>().SingleAsync(x => x.ID == item.ID);
			if (question == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Question));
				return result;
			}

			//Checking if test is in live
			result = await CheckTestIsLive(question.PaperId, result);
			if (result.HasError)
			{
				return result;
			}

			result = await CheckDuplicate(item, result);
			if (result.HasError)
			{
				return result;
			}

			var entityOptions = await UOW.GetRepositoryAsync<Choice>().GetAsync(x => x.QuestionId == item.ID);
			CollectionCompareResult<Choice> compareResult = entityOptions.Compare(item.Choices, (x, y) => x.ID.Equals(y.ID));
			foreach (var op in compareResult.Deleted)
			{
				var QA = await UOW.GetRepositoryAsync<QuestionAnswer>().SingleAsync(x => x.ChoiceId == op.ID);
				if (QA != null)
				{
					UOW.GetRepositoryAsync<QuestionAnswer>().DeleteAsync(QA);
					IsSkip = true;
				}
				UOW.GetRepositoryAsync<Choice>().DeleteAsync(op);

			}

			foreach (var op in compareResult.Added)
			{
				op.QuestionId = question.ID;
				await UOW.GetRepositoryAsync<Choice>().AddAsync(op);
			}
			foreach (var op in compareResult.Same)
			{
				var option = item.Choices.FirstOrDefault(x => x.ID == op.ID);
				op.QuestionId = question.ID;
				op.SNo = option.SNo;
				if (!option.Text.Trim().Equals(op.Text.Trim(), StringComparison.OrdinalIgnoreCase))
				{
					op.Text = option.Text.Trim();
				}
				UOW.GetRepositoryAsync<Choice>().UpdateAsync(op);
			}

			question.Text = model.Text;
			question.Type = model.Type;
			//Adding data to QuestionAnswer
			await AddOrUpdateQA(model, item, IsSkip);
			UOW.GetRepositoryAsync<Question>().UpdateAsync(question);
			return result;
		}

		private async Task AddOrUpdateQA(QuestionModel model, Question question, bool IsSkip = false)
		{
			if (!IsSkip)
			{
				var correctAns = await UOW.GetRepositoryAsync<QuestionAnswer>().GetAsync(x => x.QuestionId == model.ID);
				foreach (var option in correctAns)
				{
					UOW.GetRepositoryAsync<QuestionAnswer>().DeleteAsync(option.ID);
				}
			}

			//When question has multiple answers
			if (model.Type == (int)QuestionType.Multiple)
			{
				string[] answers = model.Key.Split(",");
				foreach (string answer in answers)
				{
					var option = question.Choices
						.FirstOrDefault(x => x.SNo == model.Choices.FirstOrDefault(x => x.Choice.Equals(answer.Trim(), StringComparison.OrdinalIgnoreCase)).SNo);

					var queAns = new QuestionAnswer();
					if (model.ID != Guid.Empty)
					{
						if (option != null && option.ID != Guid.Empty)
						{
							queAns.ChoiceId = option.ID;
						}
						queAns.QuestionId = model.ID;
					}
					else
					{
						queAns.Choice = option;
						queAns.Question = question;
					}
					await UOW.GetRepositoryAsync<QuestionAnswer>().AddAsync(queAns);
				}

			}
			else
			{
				//When Question has single answers
				var option = question.Choices.FirstOrDefault(x => x.SNo == model.Choices
				.FirstOrDefault(x => x.Choice.Equals(model.Key.Trim(), StringComparison.OrdinalIgnoreCase)).SNo);
				var queAns = new QuestionAnswer();

				if (model.ID != Guid.Empty)
				{
					if (option != null && option.ID != Guid.Empty)
					{
						queAns.ChoiceId = option.ID;
					}
					queAns.QuestionId = model.ID;
					queAns.AnswerTxt = question.Type != (int)QuestionType.Single ? model.Key : null;
				}
				else
				{
					queAns.Choice = option;
					queAns.Question = question;
					queAns.AnswerTxt = question.Type != (int)QuestionType.Single ? model.Key : null;
				}
				await UOW.GetRepositoryAsync<QuestionAnswer>().AddAsync(queAns);
			}
		}

		public async Task<Result<Question>> Delete(Guid id)
		{
			var result = new Result<Question>();
			try
			{
				var item = await UOW.GetRepositoryAsync<Question>().SingleAsync(x => x.ID == id);
				if (item != null)
				{
					//checking if paper is in live
					result = await CheckTestIsLive(item.PaperId, result);
					if (result.HasError)
					{
						return result;
					}

					item.IsDelete = true;
					UOW.GetRepositoryAsync<Question>().UpdateAsync(item);
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

		public async Task<Result<Question>> AddBulk(List<Question> entityData, List<QuestionModel> items)
		{
			var result = new Result<Question>();

			var entityQuestions = await UOW.GetRepositoryAsync<Question>()
				.GetAsync(x => x.PaperId == items.First().PaperId && !x.IsDelete, include: x => x.Include(x => x.Choices));

			CollectionCompareResult<Question> compareResult =
				entityQuestions.Compare(entityData, (x, y) => x.Text.Equals(y.Text.Trim(), StringComparison.OrdinalIgnoreCase));

			int SNo = entityQuestions.Any() ? entityQuestions.Max(x => x.SNo) : 0;
			foreach (var newItem in compareResult.Added)
			{
				newItem.SNo = ++SNo;
				var que = items.FirstOrDefault(x => x.Text.Equals(newItem.Text.Trim(), StringComparison.OrdinalIgnoreCase) && x.PaperId == newItem.PaperId);
				result = await AddQuestion(newItem.PaperId, que, newItem);
				if (result.HasError)
				{
					result.AddMessageItem(new MessageItem(result.Messages[0].Description));
					break;
				}
			}
			foreach (var editItem in compareResult.Same)
			{
				var que = items.FirstOrDefault(x => x.Text.Equals(editItem.Text.Trim(), StringComparison.OrdinalIgnoreCase) && x.PaperId == editItem.PaperId);
				var entityQue = entityData.FirstOrDefault(x => x.Text.Equals(editItem.Text.Trim(), StringComparison.OrdinalIgnoreCase) && x.PaperId == editItem.PaperId);
				entityQue.ID = editItem.ID;
				que.ID = editItem.ID;

				entityQue.Choices = AssignId(entityQue.Choices.ToList(), editItem.Choices.ToList());
				result = await UpdateQue(que, entityQue);
				if (result.HasError)
				{
					result.AddMessageItem(new MessageItem(result.Messages[0].Description));
					break;
				}
			}
			try
			{
				if (result.HasNoError)
				{
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
		private List<Choice> AssignId(List<Choice> newOp, List<Choice> OldOp)
		{
			foreach (var op in OldOp)
			{
				if (newOp.Any())
				{
					//if options already exists assigning id's
					newOp.FirstOrDefault(x => x.SNo == op.SNo).ID = op.ID;
				}
			}
			return newOp;
		}
	}
}
