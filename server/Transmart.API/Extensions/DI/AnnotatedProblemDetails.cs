namespace TranSmart.API.Extensions.DI
{
	public class AnnotatedProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
	{
		public string Code { get; set; }
	}
}
