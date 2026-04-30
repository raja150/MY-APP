using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace TranSmart.API.Extensions
{
    public class InlineFileStreamResult : FileStreamResult
    {
        public InlineFileStreamResult(Stream stream, string contentType) : base(stream, contentType)
        {

        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            base.ExecuteResult(context);
        }
    }
}
