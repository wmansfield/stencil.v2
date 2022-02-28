using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Placeholder.Common.Exceptions;

namespace Placeholder.Web.Exceptions
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public HttpResponseExceptionFilter()
        {
            this.Order = int.MaxValue - 50;
        }
        public int Order { get; private set; }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException exception)
            {
                context.Result = new ObjectResult(exception.Content)
                {
                    StatusCode = (int)exception.Status,
                };
                context.ExceptionHandled = true;
            }
        }
    }

}
