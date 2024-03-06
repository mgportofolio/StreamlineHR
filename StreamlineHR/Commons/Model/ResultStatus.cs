using Microsoft.AspNetCore.Mvc;

namespace StreamlineHR.Commons.Model
{
    public class ResultStatus<T> : ActionResult where T : class
    {
        public ResultStatus()
        {
        }

        public ResultStatus(bool status, string? message)
        {
            Status = status;
            Message = message;
        }

        public bool Status { get; set; } = true;
        public string? Message { get; set; } = "Success";
        public int Code { get; set; } = 200;
        public T? Data { get; set; }
        public override Task ExecuteResultAsync(ActionContext context)
        {
            var result = new ObjectResult(this)
            {
                StatusCode = Code
            };
            return result.ExecuteResultAsync(context);
        }
    }
}
