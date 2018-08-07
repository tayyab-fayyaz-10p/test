using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSH.Core.Infrastructure
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
            throw ex;
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
            throw new Exception(content);
        }

        public void ThrowAPIException(List<string> content)
        {
            this.ThrowAPIException(HttpStatusCode.BadRequest, content);
        }

        public void ThrowAPIException(HttpStatusCode code, List<string> content)
        {
            this.ThrowAPIException(HttpStatusCode.BadRequest, string.Empty, content);
        }

        public void ThrowAPIException(HttpStatusCode code, string reasonPhrase, List<string> content)
        {
            throw new Exception(string.Join(", ", content.ToArray()));
        }
    }
}
