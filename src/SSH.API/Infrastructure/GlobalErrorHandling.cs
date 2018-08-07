using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using SSH.Common.Helper;

namespace SSH.API.Infrastructure
{
    public class GlobalErrorHandling : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            string errorMessage = context.Exception.Message;
            var innerException = context.Exception.InnerException;
            var response = context.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    (object)Serializer.CreateObject(HttpStatusCode.InternalServerError, errorMessage, null));

            ExceptionHelper helper = new ExceptionHelper();

            helper.LogAPIException(new System.Exception(errorMessage, innerException));

            //response.Headers.Add("Error", string.Format("{0} :  {1} ", errorMessage.Replace(Environment.NewLine, " "), innerException == null || innerException.InnerException == null ? string.Empty : innerException.InnerException.Message));
            //response.ReasonPhrase = errorMessage.Replace(Environment.NewLine, " ");
            context.Result = new ResponseMessageResult(response);
        }
    }
}