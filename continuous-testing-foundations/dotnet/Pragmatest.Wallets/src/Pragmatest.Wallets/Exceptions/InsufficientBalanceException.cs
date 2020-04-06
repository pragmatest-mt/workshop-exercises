using System;
using System.Runtime.Serialization;

namespace Pragmatest.Wallets.Exceptions
{
    [Serializable]
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException() : base ("Invalid withdrawal amount.There are insufficient funds.")
        {
        }

        public InsufficientBalanceException(string message) : base(message)
        {
        }

        public InsufficientBalanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InsufficientBalanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
