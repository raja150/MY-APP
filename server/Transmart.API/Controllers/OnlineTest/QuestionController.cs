using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Models.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Domain.Models.OnlineTest.Response;
using TranSmart.Service.OnlineTest;
using QuestionType = TranSmart.Domain.Enums.QuestionType;

namespace TranSmart.API.Controllers.OnlineTest
{
	[Route("api/OnlineTest/[controller]")]
	[ApiController]
	public class QuestionController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IQuestionService _service;
		public QuestionController(IMapper mapper, IQuestionService service)
		{
			_mapper = mapper;
			_service = service;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			return Ok(await _service.GetQueById(id));
		}


		[HttpPost]
		public async Task<IActionResult> Post(QuestionModel model)
		{
			var result = await _service.Add(_mapper.Map<Question>(model), model, model.PaperId);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpPut]
		public async Task<IActionResult> Put(QuestionModel model)
		{
			var entity = _mapper.Map<Question>(model);
			var result = await _service.Update(entity, model);
			if (result.IsSuccess)
			{
				return Ok();
			}
			return BadRequest(result);
		}

		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] PaperSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<QuestionsList>>(await _service.GetPaginate(baseSearch)));
		}


		[HttpPut("delete")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var result = await _service.Delete(id);
			if (result.IsSuccess)
			{
				return Ok();
			}
			return BadRequest(result);
		}

		[HttpGet("Export/{paperId}")]
		public async Task<IActionResult> Export(Guid paperId)
		{
			var result = new Result<Question>();

			var queAnswers = await _service.GetQuestionAnswers(paperId);
			//if (!queAnswers.Any())
			//{
			//	result.AddMessageItem(new MessageItem("No questions found"));
			//	return BadRequest(result);
			//}

			var ms = new MemoryStream();
			var workbook = new XLWorkbook();
			var workSheet = workbook.Worksheets.Add("Questions");

			string[] Headers = { "Question", "Type", "Key", "Option A", "Option B", "Option C", "Option D", "Option E", "Option F" };
			ClosedXmlGeneric.AddHeaders(workSheet, Headers);

			//Sheet row number
			int RowNo = 2;
			foreach (var item in queAnswers.DistinctBy(x => x.QuestionId))
			{
				var key = "";
				if (item.Question.Type == (int)QuestionType.Single)
				{
					key = ConstUtil.GetOption(item.Choice.SNo);
				}
				else if (item.Question.Type == (int)QuestionType.Multiple)
				{
					var answers = queAnswers.Where(x => x.QuestionId == item.QuestionId).ToList();
					foreach (var ans in answers)
					{
						key += ConstUtil.GetOption(ans.Choice.SNo) + ",";
					}
					key = key.Remove(key.Length - 1);
				}
				else
				{
					key = item.AnswerTxt;
				}

				workSheet.Cell(RowNo, 1).Value = Regex.Replace(item.Question.Text, "<.*?>", String.Empty);//Removing html tags
				workSheet.Cell(RowNo, 2).Value = ConstUtil.GetQuestionType(item.Question.Type);
				workSheet.Cell(RowNo, 3).Value = key;
				workSheet.Cell(RowNo, 4).Value = item.Question.Choices.FirstOrDefault(x => x.SNo == 1)?.Text;
				workSheet.Cell(RowNo, 5).Value = item.Question.Choices.FirstOrDefault(x => x.SNo == 2)?.Text;
				workSheet.Cell(RowNo, 6).Value = item.Question.Choices.FirstOrDefault(x => x.SNo == 3)?.Text;
				workSheet.Cell(RowNo, 7).Value = item.Question.Choices.FirstOrDefault(x => x.SNo == 4)?.Text;
				workSheet.Cell(RowNo, 8).Value = item.Question.Choices.FirstOrDefault(x => x.SNo == 5)?.Text;
				workSheet.Cell(RowNo, 9).Value = item.Question.Choices.FirstOrDefault(x => x.SNo == 6)?.Text;
				RowNo++;
			}

			workSheet.Columns().AdjustToContents();
			workbook.SaveAs(ms);
			ms.Seek(0, SeekOrigin.Begin);
			return new DownloadFile(ms, "Questions");
		}
	}
}
