using Microsoft.EntityFrameworkCore;
using Pragmatest.Wallets.Data.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Pragmatest.Wallets.Data.IntegrationTests")]

namespace Pragmatest.Wallets.Data.Repositories
{
    internal class WalletRepository : IWalletRepository
    {
        private readonly WalletContext _walletContext;

        public WalletRepository(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }
                
        public async Task<WalletEntry> GetLastWalletEntryAsync()
        {
            return await _walletContext
                .Transactions
                .OrderByDescending(walletEntry => walletEntry.EventTime)
                .FirstOrDefaultAsync();
        }

        public Task InsertWalletEntryAsync(WalletEntry walletEntry)
        {
            _walletContext.Transactions.Add(walletEntry);
            _walletContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
