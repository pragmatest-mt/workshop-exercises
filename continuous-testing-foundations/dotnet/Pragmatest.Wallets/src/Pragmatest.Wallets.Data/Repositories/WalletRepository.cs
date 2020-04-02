using Pragmatest.Wallets.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pragmatest.Wallets.Data.Repositories
{
    internal class WalletRepository : IWalletRepository
    {
        private readonly List<WalletEntry> _walletEntries;

        public WalletRepository()
        {
            _walletEntries = new List<WalletEntry>();
        }

        public Task<List<WalletEntry>> GetWalletEntriesAsync()
        {
            return Task.FromResult(_walletEntries);
        }
        
        public Task<WalletEntry> GetLastWalletEntryAsync()
        {
            return Task.FromResult(_walletEntries.OrderByDescending(walletEntry => walletEntry.EventTime).FirstOrDefault());
        }

        public Task InsertWalletEntryAsync(WalletEntry walletEntry)
        {
            _walletEntries.Add(walletEntry);
            return Task.CompletedTask;
        }
    }
}
