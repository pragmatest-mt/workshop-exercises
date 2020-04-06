using System;

namespace Pragmatest.Wallets.Data.Models
{
    public class WalletEntry
    {
        public string Id { get; set; }
        public DateTimeOffset EventTime { get; set; } = DateTimeOffset.Now.UtcDateTime;

        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }

        public WalletEntry()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
