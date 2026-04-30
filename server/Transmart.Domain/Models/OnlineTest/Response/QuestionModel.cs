using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class QuestionRequest
	{
		public ICollection<QuestionModel> Questions { get; set; }
		public Guid PaperId { get; set; }
	}
	public class QuestionModel : BaseModel
	{
		public string Text { get; set; }
		public byte Type { get; set; }
		public ICollection<ChoiceModel> Choices { get; set; }
		public string Key { get; set; }
		public Guid PaperId { get; set; }
	}
	public class ChoiceModel : BaseModel
	{
		public byte SNo { get; set; }
		public string Choice { get; set; }
		public string Text { get; set; }
		
	}


	public class OptionTextModelValidator : AbstractValidator<ChoiceModel>
	{
		public OptionTextModelValidator()
		{
			RuleFor(x => x.Text).NotEmpty().NotNull().WithMessage("Option Required");
		}
	}

	public class QuestionModelValidator : AbstractValidator<QuestionModel>
	{
		public QuestionModelValidator()
		{
			RuleFor(m => m.Key).NotNull().NotEmpty().WithMessage("Key required");
			RuleFor(m => m.Text).NotEmpty().NotNull().WithMessage("Question required");
			RuleForEach(m => m.Choices).SetValidator(m => new OptionTextModelValidator());
			RuleFor(m => m).Must(m => !m.Choices.GroupBy(p => p.Text.Trim()).Any(g => g.Count() > 1))
				.WithMessage("Duplicate Choices not allowed");
		}
	}

	public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
	{
		public QuestionRequestValidator()
		{
			RuleForEach(m => m.Questions).SetValidator(m => new QuestionModelValidator());
			RuleFor(m => m).Must(m => !m.Questions.GroupBy(q => q.Text.Trim()).Any(g => g.Count() > 1))
				.WithMessage("Question already exists");
		}
	}

}
