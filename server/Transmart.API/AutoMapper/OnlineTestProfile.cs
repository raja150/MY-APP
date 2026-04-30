using TranSmart.Core.Util;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Models.Cache.OnlineTest;
using TranSmart.Domain.Models.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Domain.Models.OnlineTest.Request;
using TranSmart.Domain.Models.OnlineTest.Response;

namespace TranSmart.API.AutoMapper
{
	public class OnlineTestProfile : Profile
	{
		public OnlineTestProfile()
		{
			CreateMap<Question, QuestionModel>();
			CreateMap<QuestionModel, Question>();

			CreateMap<ResultQuestion, QuestionModel>();
			CreateMap<QuestionModel, ResultQuestion>();

			CreateMap<Question, QuestionsList>()
				.ForMember(d => d.Type, opt => opt.MapFrom(x => ConstUtil.GetQuestionType(x.Type)))
				.ForMember(d => d.Paper, opt => opt.MapFrom(x => x.Paper.Name));

			CreateMap<ResultQuestion, TestModel>()
				.ForMember(d => d.Text, s => s.MapFrom(x => x.Question.Text))
				.ForMember(d => d.Type, s => s.MapFrom(x => x.Question.Type))
				.ForMember(d => d.TestName, s => s.MapFrom(x => x.Paper.Name))
				.ForMember(d => d.TestDate, s => s.MapFrom(x => x.Paper.StartAt))
				.ForMember(d => d.Duration, s => s.MapFrom(x => x.Paper.Duration))
				.ForMember(d => d.Choices, s => s.MapFrom(x => x.Question.Choices));


			CreateMap<ChoiceModel, Choice>();
			CreateMap<Choice, ChoiceModel>();

			CreateMap<ResultQuestion, TestAnswerRequest>();
			CreateMap<TestAnswerRequest, ResultQuestion>();

			CreateMap<Paper, PaperList>();

			CreateMap<Paper, PaperModel>();
			CreateMap<PaperModel, Paper>();

			CreateMap<Question, PaperModel>()
				.ForMember(d => d.ID, s => s.MapFrom(x => x.PaperId))
				.ForMember(d => d.Name, s => s.MapFrom(x => x.Paper.Name))
				.ForMember(d => d.OrganiserId, s => s.MapFrom(x => x.Paper.OrganiserId))
				.ForMember(d => d.StartAt, s => s.MapFrom(x => x.Paper.StartAt))
				.ForMember(d => d.EndAt, s => s.MapFrom(x => x.Paper.EndAt))
				.ForMember(d => d.Duration, s => s.MapFrom(x => x.Paper.Duration))
				.ForMember(d => d.IsJumbled, s => s.MapFrom(x => x.Paper.IsJumbled))
				.ForMember(d => d.MoveToLive, s => s.MapFrom(x => x.Paper.MoveToLive))
				.ForMember(d => d.Status, s => s.MapFrom(x => x.Paper.Status));

			CreateMap<Paper, PaperRequest>();
			CreateMap<PaperRequest, Paper>();

			//Cache
			CreateMap<Paper, PaperSearchCache>()
				.ForMember(d => d.PaperId, s => s.MapFrom(x => x.ID))
				.ForMember(d => d.Paper,s => s.MapFrom(x => x.Name));


			//For Result
			CreateMap<Result, ResultList>()
				.ForMember(d => d.Date, opt => opt.MapFrom(x => x.AddedAt))
				.ForMember(d => d.Employee, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.ShowResult, opt => opt.MapFrom(x => x.Paper.ShowResult))
				.ForMember(d => d.EmployeeId, opt => opt.MapFrom(x => x.Employee.ID));

			CreateMap<Result, ResultPaperModel>()
				.ForMember(d => d.ID, opt => opt.MapFrom(x => x.PaperId))
				.ForMember(d => d.PaperName, opt => opt.MapFrom(x => x.Paper.Name));

			CreateMap<Result, ResultEmpModel>()
				.ForMember(d => d.ID, opt => opt.MapFrom(x => x.EmployeeId))
				.ForMember(d => d.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name));


			CreateMap<Paper, TestList>();

			CreateMap<TestEmployee, TestEmployeeRequest>();
			CreateMap<TestEmployeeRequest, TestEmployee>();

			CreateMap<TestEmployee, TestEmployeeModel>();
			CreateMap<TestEmployeeModel, TestEmployee>();

			CreateMap<TestEmployee, TestEmployeeList>()
				.ForMember(d => d.Employee, src => src.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.EmployeeCode, src => src.MapFrom(x => x.Employee.No))
				.ForMember(d => d.EmployeeDept, src => src.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.EmployeeDesig, src => src.MapFrom(x => x.Employee.Designation.Name));

			CreateMap<TestDesignation, TestDesignationRequest>();
			CreateMap<TestDesignationRequest, TestDesignation>();

			CreateMap<TestDesignation, TestDesignationModel>();
			CreateMap<TestDesignationModel, TestDesignation>();

			CreateMap<TestDesignation, TestDesignationList>()
				.ForMember(d => d.Designation, src => src.MapFrom(x => x.Designation.Name));

			CreateMap<TestDepartment, TestDepartmentRequest>();
			CreateMap<TestDepartmentRequest, TestDepartment>();

			CreateMap<TestDepartment, TestDesignationModel>();
			CreateMap<TestDepartmentModel, TestDepartment>();

			CreateMap<TestDepartment, TestDepartmentList>()
				.ForMember(d => d.Department, src => src.MapFrom(x => x.Department.Name));

			CreateMap<ResultQuestion, AnswerModel>()
				.ForMember(d => d.AnswerTxt, src => src.MapFrom(x => x.Answer));
		}
	}
}
