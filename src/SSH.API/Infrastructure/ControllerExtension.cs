using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using SSH.Common.Helper;

namespace SSH.API.Infrastructure
{
    public static class ControllerExtensions
    {
        public static async Task<ResponseMessageResult> JsonApiSuccessAsync<T>(this T controller, string message, object obj) where T : Recipe.Core.Base.Generic.Controller
        {
            return await CreateResponses<T>(controller, HttpStatusCode.OK, message, obj);
        }

        public static async Task<ResponseMessageResult> JsonApiSuccessAsync<T>(this T controller, string message, object obj, string dateTime) where T : Recipe.Core.Base.Generic.Controller
        {
            return await CreateResponses<T>(controller, HttpStatusCode.OK, message, obj, dateTime);
        }

        public static async Task<ResponseMessageResult> JsonApiBadRequestAsync<T>(this T controller, string message, object obj) where T : Recipe.Core.Base.Generic.Controller
        {
            return await CreateResponses<T>(controller, HttpStatusCode.BadRequest, message, obj);
        }

        public static async Task<ResponseMessageResult> JsonApiNotFoundAsync<T>(this T controller, string message, object obj) where T : Recipe.Core.Base.Generic.Controller
        {
            return await CreateResponses<T>(controller, HttpStatusCode.NotFound, message, obj);
        }

        public static async Task<ResponseMessageResult> JsonApiNoContentAsync<T>(this T controller, string message, object obj) where T : Recipe.Core.Base.Generic.Controller
        {
            return await CreateResponses<T>(controller, HttpStatusCode.NoContent, message, obj);
        }

        private static Task<ResponseMessageResult> CreateResponses<T>(this T controller, HttpStatusCode code, string message, object obj) where T : Recipe.Core.Base.Generic.Controller
        {
            var response = controller.Request.CreateResponse(code, (object)Serializer.CreateObject(code, message, obj));
            var result = controller.GetResponseMessage(response);
            return result;
        }

        private static Task<ResponseMessageResult> CreateResponses<T>(this T controller, HttpStatusCode code, string message, object obj, string dateTime) where T : Recipe.Core.Base.Generic.Controller
        {
            var response = controller.Request.CreateResponse(code, (object)Serializer.CreateObject(code, message, obj, dateTime));
            var result = controller.GetResponseMessage(response);
            return result;
        }
    }
}