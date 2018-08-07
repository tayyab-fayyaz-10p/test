using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using SSH.Common.Helper;
using SSH.Core.Infrastructure;

namespace SSH.API.Infrastructure
{
    public class ExceptionHelper : IExceptionHelper
    {
        public Exception GetAPIException(HttpStatusCode code, string reasonPhrase, string content)
        {
            var resp = new HttpResponseMessage(code);

            resp.Content = new StringContent(content);
            resp.ReasonPhrase = reasonPhrase;

            return new HttpResponseException(resp);
        }

        public void LogAPIException(Exception ex)
        {
            Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex));
        }

        public void LogAPIException(string log)
        {
            Exception ex = new Exception(log);
            this.LogAPIException(ex);
        }

        public void LogFileException(Exception ex)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Exceptions\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (TextWriter tw = new StreamWriter(path + DateTime.UtcNow.ToString().Replace("/", "_").Replace(":", "_") + ".txt"))
            {
                tw.WriteLine(ex.Message);
            }
        }

        public void LogFileException(string log)
        {
            Exception ex = new Exception(log);
            this.LogFileException(ex);
        }

        public void ThrowAPIException(string content)
        {
            this.ThrowAPIException(HttpStatusCode.BadRequest, string.Empty, content);
        }

        public void ThrowAPIException(HttpStatusCode code, string content)
        {
            this.ThrowAPIException(code, string.Empty, content);
        }

        public void ThrowAPIException(HttpStatusCode code, string reasonPhrase, string content)
        {
            var resp = new HttpResponseMessage(code);

            var error = Serializer.CreateObject(code, content, null);

            resp.Content = new ObjectContent(error.GetType(), error, new JsonMediaTypeFormatter());

            Exception ex = new Exception(content);
            this.LogAPIException(ex);

            throw new HttpResponseException(resp);
        }

        public void ThrowAPIException(List<string> content)
        {
            this.ThrowAPIException(HttpStatusCode.BadRequest, content);
        }

        public void ThrowAPIException(HttpStatusCode code, List<string> content)
        {
            this.ThrowAPIException(code, string.Empty, content);
        }

        public void ThrowAPIException(HttpStatusCode code, string reasonPhrase, List<string> content)
        {
            var resp = new HttpResponseMessage(code);

            List<dynamic> error = new List<dynamic>();

            foreach (var item in content)
            {
                error.Add(Serializer.CreateObject(code, item, null));

                Exception ex = new Exception(item);
                this.LogAPIException(ex);
            }

            resp.Content = new ObjectContent(error.GetType(), error, new JsonMediaTypeFormatter());

            throw new HttpResponseException(resp);
        }
    }
}