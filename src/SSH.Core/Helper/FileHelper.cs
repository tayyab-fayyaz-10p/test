using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using SSH.Core.DTO;

namespace SSH.Core.Helper
{
    public class FileHelper
    {
        public static HttpResponseMessage DownloadFile(FileDTO doc, int cacheControlTime)
        {
            if (doc != null && !string.IsNullOrEmpty(doc.Data))
            {
                byte[] bytes = Convert.FromBase64String(doc.Data);

                var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var bytesHash = md5.ComputeHash(bytes);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(bytes);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(doc.Type);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                response.Content.Headers.ContentDisposition.FileName = doc.SourceFileName;
                response.Headers.ETag = new EntityTagHeaderValue("\"" + BitConverter.ToString(bytesHash) + "\"");
                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = new TimeSpan(0, cacheControlTime, 0)
                };
                return response;
            }

            return null;
        }
    }
}
