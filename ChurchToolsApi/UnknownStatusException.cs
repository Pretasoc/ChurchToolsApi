using System;
using System.Runtime.Serialization;

namespace ChurchToolsApi
{
    [Serializable]
    internal class UnknownStatusException : Exception
    {
        public UnknownStatusException()
        {
        }

        public UnknownStatusException(string message) : base(message)
        {
        }

        public UnknownStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownStatusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}