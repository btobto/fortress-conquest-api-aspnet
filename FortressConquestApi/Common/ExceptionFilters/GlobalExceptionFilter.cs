using FortressConquestApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;

namespace FortressConquestApi.Common.ExceptionFilters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HttpStatusCode? statusCode = context.Exception switch
            {
                ItemNotFoundException => HttpStatusCode.NotFound,
                FortressPlacementForbiddenException => HttpStatusCode.Conflict,
                EmailTakenException => HttpStatusCode.Conflict,
                ArgumentOutOfRangeException => HttpStatusCode.BadRequest,
                _ => null
            };

            if (statusCode != null)
            {
                int status = (int)statusCode;

                var problemDetails = new ProblemDetails
                {
                    Title = ReasonPhrases.GetReasonPhrase(status),
                    Status = status
                };

                if (context.Exception.Message != "")
                {
                    problemDetails.Detail = context.Exception.Message;
                }

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }
        }
    }
}
