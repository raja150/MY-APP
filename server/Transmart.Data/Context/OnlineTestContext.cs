using Microsoft.EntityFrameworkCore;

using TranSmart.Domain.Entities.OnlineTest;

namespace TranSmart.Data
{
	public partial class TranSmartContext
	{
		public DbSet<Question> Question { get; set; }
		public DbSet<Paper> Paper { get; set; }
		public DbSet<Choice> Choice { get; set; }
		public DbSet<Result> Result { get; set; }
		public DbSet<ResultQuestion> ResultQuestion { get; set; }
		public DbSet<TestDepartment> TestDepartment { get; set; }
		public DbSet<TestDesignation> TestDesignation { get; set; }
		public DbSet<TestEmployee> TestEmployee { get; set; }
		public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
	}
}
