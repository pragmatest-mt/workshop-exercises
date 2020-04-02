using Microsoft.EntityFrameworkCore;
using Pragmatest.Wallets.Data.Models;

namespace Pragmatest.Wallets.Data
{
    interface IWalletContext
    {
        DbSet<WalletEntry> Transactions { get; set; }
        int SaveChanges();
    }
}
