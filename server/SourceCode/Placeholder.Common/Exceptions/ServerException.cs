using System;

namespace Placeholder.Common.Exceptions
{
    [System.Serializable]
    public class ServerException : System.Exception
    {
        public ServerException() { }
        public ServerException(string message) : base(message) { }
        public ServerException(string message, System.Exception inner) : base(message, inner) { }
        protected ServerException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
