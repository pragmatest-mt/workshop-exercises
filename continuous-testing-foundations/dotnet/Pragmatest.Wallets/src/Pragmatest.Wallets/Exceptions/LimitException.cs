using System;
using System.Runtime.Serialization;

namespace Pragmatest.Wallets.Exceptions
{
    [Serializable]
    public class LimitException : Exception
    {
        public LimitException() : base("Invalid amount. Outside allowed limit")
        {
        }

        public LimitException(string message) : base(message)
        {
        }

        public LimitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LimitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
