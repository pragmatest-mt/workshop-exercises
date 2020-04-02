using System;
using System.Collections.Generic;
using System.Text;

namespace Pragmatest.Wallets.Data.Models
{
    public class WalletEntry
    {
        public DateTimeOffset EventTime { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }

    }
}
