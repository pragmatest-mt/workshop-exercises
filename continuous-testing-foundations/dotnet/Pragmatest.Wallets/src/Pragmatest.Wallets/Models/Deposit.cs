using System;

namespace Pragmatest.Wallets.Models
{
    public class Deposit
    {
        public decimal Amount { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Deposit d = (Deposit)obj;
                return (Amount == d.Amount);
            }
        }
    }
}
