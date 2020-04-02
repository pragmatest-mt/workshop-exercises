using Pragmatest.Wallets.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pragmatest.Wallets.Data.Repositories
{
    public interface IWalletRepository
    {
        Task<WalletEntry> GetLastWalletEntryAsync();
        Task InsertWalletEntryAsync(WalletEntry walletEntry);
    }
}
