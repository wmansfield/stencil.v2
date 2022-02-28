using System;
using System.Net;

namespace Placeholder.SDK.Client.Exceptions
{
    public class EndpointTimeoutException : EndpointException
    {
        public EndpointTimeoutException() : base() { }
        public EndpointTimeoutException(HttpStatusCode statusCode) : base(statusCode) { }
        public EndpointTimeoutException(HttpStatusCode statusCode, string message) : base(statusCode, message) { }
        public EndpointTimeoutException(HttpStatusCode statusCode, string message, Exception inner) : base(statusCode, message, inner) { }

    }
}
