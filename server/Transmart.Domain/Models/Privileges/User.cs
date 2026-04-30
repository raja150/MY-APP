using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TranSmart.Domain.Models.SelfService;

namespace TranSmart.Domain.Models
{

	public class UserModel : BaseModel
	{
		public string Name { get; set; }
		public Guid RoleId { get; set; }
		public string Role { get; set; }
		public string Password { get; set; }
		public Guid EmployeeId { get; set; }
		public int Type => 1;
	}
	public class UserModelValidator : AbstractValidator<UserModel>
	{
		public UserModelValidator()	
		{
			RuleFor(m => m.Password).Length(5, 18).When(x => !string.IsNullOrEmpty(x.Password))
				.WithMessage("password should be in between 5 to 18 characters");
		}
	}
	public class UsersList : BaseModel
	{
		public Guid? EmployeeID { get; set; }	
		public string Name { get; set; }
		public string Role { get; set; }
		public string EmpName { get; set; }
		public string EmpCode { get; set; }
		public string Designation { get; set; }
		public DateTime LastLogin { get; set; }
		public DateTime ExpireOn { get; set; }
		public string Department { get; set; }
	}
	public class SsoUserModel
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; }
		public DateTime ExpireOn { get; set; }
		public string Password { get; set; }
		public int Type { get; set; }
	}
}
