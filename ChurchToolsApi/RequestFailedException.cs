using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace ChurchToolsApi
{
    [Serializable]
    internal class RequestFailedException : Exception
    {
        private JObject responseObject;

        public RequestFailedException()
        {
        }

        public RequestFailedException(JObject responseObject)
        {
            this.responseObject = responseObject;
        }

        public RequestFailedException(string message) : base(message)
        {
        }

        public RequestFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RequestFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}