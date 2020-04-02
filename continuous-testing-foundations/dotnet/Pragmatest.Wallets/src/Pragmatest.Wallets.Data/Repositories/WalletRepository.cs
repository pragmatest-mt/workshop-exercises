using Pragmatest.Wallets.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Pragmatest.Wallets.Data.Repositories
{
    internal class WalletRepository : IWalletRepository
    {
        private readonly IWalletContext _walletContext;

        public WalletRepository(IWalletContext walletContext)
        {
            _walletContext = walletContext;
        }
                
        public Task<WalletEntry> GetLastWalletEntryAsync()
        {
            return Task.FromResult(
                _walletContext.Transactions
                .OrderByDescending(walletEntry => walletEntry.EventTime)
                .FirstOrDefault());
        }

        public Task InsertWalletEntryAsync(WalletEntry walletEntry)
        {
            _walletContext.Transactions.Add(walletEntry);
            _walletContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
