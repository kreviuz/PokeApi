using System;
using System.Net;

namespace PokeApi.Infrastracture.Exceptions
{
    public class StatusCodeException : Exception
    {
        public StatusCodeException(HttpStatusCode statusCode, string serviceMessage)
        {
            StatusCode = statusCode;
            ServiceMessage = serviceMessage;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string ServiceMessage { get; set; }
    }
}
