using System;
using System.Net;

namespace Placeholder.Common.Exceptions
{
    [System.Serializable]
    public class HttpResponseException : Exception
    {
        public HttpResponseException(HttpStatusCode statusCode)
        {
            this.Status = statusCode;
        }
        public HttpStatusCode Status { get; set; }
        public object Content { get; set; }
    }
}
