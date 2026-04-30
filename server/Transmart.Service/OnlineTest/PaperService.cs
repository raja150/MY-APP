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

namespace TranSmart.Service.OnlineTest
{
	public interface IPaperService : IBaseService<Paper>
	{
		Task<Result<Paper>> Duplicate(Guid paperId);
		Task<IEnumerable<Paper>> GetPapers(Guid empId);

	}
	public class PaperService : BaseService<Paper>, IPaperService
	{
		public PaperService(IUnitOfWork uow) : base(uow)
		{

		}

		//filter in question list
		public async Task<IEnumerable<Paper>> GetPapers(Guid empId)
		{
			return await UOW.GetRepositoryAsync<Paper>().GetAsync(x => !x.MoveToLive && x.Status && x.OrganiserId == empId);
		}


		public override async Task<IPaginate<Paper>> GetPaginate(BaseSearch baseSearch)
		{
			var search = (PaperSearch)baseSearch;
			return await UOW.GetRepositoryAsync<Paper>().GetPageListAsync(
				predicate: x => (!search.PaperId.HasValue || x.ID == search.PaperId) &&
				(!search.Status.HasValue || (search.Status == 1 ? x.Status : !x.Status)) &&
				(!search.IsinLive.HasValue || (search.IsinLive == 1 ? x.MoveToLive : !x.MoveToLive)),
				index: search.Page,
				size: search.Size,
				sortBy: search.SortBy ?? "StartAt",
				ascending: search.IsDescend);
		}

		public async Task<Result<Paper>> Duplicate(Guid paperId)
		{
			var result = new Result<Paper>();

			try
			{
				var questionAnwers = await UOW.GetRepositoryAsync<QuestionAnswer>().GetAsync(
					x => x.Question.PaperId == paperId && !x.Question.IsDelete,
						include: x => x.Include(x => x.Question).ThenInclude(x => x.Paper)
						.Include(x => x.Question).ThenInclude(x => x.Choices)
						.Include(x => x.Choice));

				if (questionAnwers.Any())
				{
					var paperEntity = questionAnwers.First().Question.Paper;
					var newPaper = new Paper
					{
						Duration = paperEntity.Duration,
						IsJumbled = paperEntity.IsJumbled,
						Name = paperEntity.Name + "_" + TimeStamp(),
						StartAt = paperEntity.StartAt,
						EndAt = paperEntity.EndAt,
						OrganiserId = paperEntity.OrganiserId,
					};
					await UOW.GetRepositoryAsync<Paper>().AddAsync(newPaper);

					var duplicateQuestions = new List<Question>();
					foreach (var queAns in questionAnwers.DistinctBy(x => x.QuestionId))
					{
						queAns.Question.ID = Guid.NewGuid();
						queAns.Question.Paper = newPaper;
						var newChoice = new List<Choice>();
						if (queAns.Question.Type == (int)QuestionType.Single || queAns.Question.Type == (int)QuestionType.Multiple)
						{
							foreach (var item in queAns.Question.Choices)
							{
								newChoice.Add(new Choice
								{
									SNo = item.SNo,
									QuestionId = queAns.Question.ID,
									Text = item.Text,
									ID = Guid.NewGuid(),
								});
							}
							queAns.Question.Choices = newChoice;
						}
						duplicateQuestions.Add(queAns.Question);

						if (queAns.Question.Type == (int)QuestionType.Multiple)
						{
							foreach (var item in questionAnwers.Where(x => x.QuestionId == queAns.QuestionId))
							{
								await UOW.GetRepositoryAsync<QuestionAnswer>().AddAsync(new QuestionAnswer
								{
									QuestionId = queAns.Question.ID,
									ChoiceId = newChoice.FirstOrDefault(x => x.SNo == item.Choice.SNo).ID
								});
							}
						}
						else
						{
							await UOW.GetRepositoryAsync<QuestionAnswer>().AddAsync(new QuestionAnswer
							{
								AnswerTxt = queAns.Question.Type != (int)QuestionType.Single ? queAns.AnswerTxt : null,
								ChoiceId = queAns.Question.Type == (int)QuestionType.Single ? newChoice.FirstOrDefault(x => x.SNo == queAns.Choice.SNo).ID : null,
								QuestionId = queAns.Question.ID,
							});
						}
					}
					await UOW.GetRepositoryAsync<Question>().AddAsync(duplicateQuestions);
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
				}
				else
				{
					result.AddMessageItem(new MessageItem(Resource.No_Questions_Found));
				}
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public override async Task CustomValidation(Paper item, Result<Paper> result)
		{
			if (await UOW.GetRepositoryAsync<Paper>().HasRecordsAsync(x => x.Name.ToLower().Equals(item.Name.ToLower().Trim())
																		&& x.ID != item.ID && x.Status))
			{
				result.AddMessageItem(new MessageItem(Resource.Paper_Name_Already_Exist));
			}
			if (item.MoveToLive && !await UOW.GetRepositoryAsync<Question>().HasRecordsAsync(x => x.PaperId == item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Please_Add_Questions));
			}
			if (item.ShowResult && !await UOW.GetRepositoryAsync<Result>().HasRecordsAsync(x => x.PaperId == item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.No_Result_Found));
			}
			var entity = await UOW.GetRepositoryAsync<Paper>().SingleAsync(x => x.ID == item.ID);
			if (await UOW.GetRepositoryAsync<ResultQuestion>().HasRecordsAsync(x => x.PaperId == item.ID) && !entity.IsEqual(item))
			{
				result.AddMessageItem(new MessageItem(Resource.Paper_is_in_live));
			}
			await base.CustomValidation(item, result);
		}

		public override async Task<Result<Paper>> UpdateAsync(Paper item)
		{
			var result = new Result<Paper>();
			var entity = await UOW.GetRepositoryAsync<Paper>().SingleAsync(x => x.ID == item.ID);
			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Paper_Not_Exist));
				return result;
			}
			entity.Update(item);
			return await base.UpdateAsync(entity);
		}
	}
}
