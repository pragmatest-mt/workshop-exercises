using System;
using System.Collections.Generic;
using System.Text;

namespace Pragmatest.Wallets.Models
{
    public class Withdrawal
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
                Withdrawal d = (Withdrawal)obj;
                return (Amount == d.Amount);
            }
        }
    }
}
