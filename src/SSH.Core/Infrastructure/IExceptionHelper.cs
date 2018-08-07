using System;
using System.Collections.Generic;
using System.Net;

namespace SSH.Core.Infrastructure
{
    public interface IExceptionHelper
    {
        void ThrowAPIException(string content);

        void ThrowAPIException(HttpStatusCode code, string content);

        void ThrowAPIException(HttpStatusCode code, string reasonPhrase, string content);

        void ThrowAPIException(List<string> content);

        void ThrowAPIException(HttpStatusCode code, List<string> content);

        void ThrowAPIException(HttpStatusCode code, string reasonPhrase, List<string> content);

        Exception GetAPIException(HttpStatusCode code, string reasonPhrase, string content);

        void LogAPIException(string log);

        void LogAPIException(Exception ex);

        void LogFileException(string log);

        void LogFileException(Exception ex);
    }
}
