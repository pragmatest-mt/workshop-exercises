using Pragmatest.Wallets.Data.Models;
using Pragmatest.Wallets.Data.Repositories;
using Pragmatest.Wallets.Models;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Pragmatest.Wallets.UnitTests")]

namespace Pragmatest.Wallets.Services
{
    internal class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Balance> GetBalanceAsync()
        {
            WalletEntry walletEntry = await _walletRepository.GetLastWalletEntryAsync();

            decimal amount = walletEntry == default(WalletEntry) ? 0 : (walletEntry.BalanceBefore + walletEntry.Amount);
            
            Balance currentBalance = new Balance
            {
                Amount = amount
            };

            return currentBalance;
        }

        public async Task<Balance> DepositFundsAsync(Deposit deposit)
        {
            decimal entryAmount = deposit.Amount;
            Balance currentBalance = await GetBalanceAsync();
            decimal currentBalanceAmount = currentBalance.Amount;
            
            WalletEntry depositEntry = new WalletEntry()
            {
                Amount = entryAmount,
                BalanceBefore = currentBalanceAmount,
                EventTime = DateTimeOffset.UtcNow
            };

            await _walletRepository.InsertWalletEntryAsync(depositEntry);

            Balance newBalance = new Balance
            {
                Amount = currentBalanceAmount + entryAmount
            };

            return newBalance;
        }

        public async Task<Balance> WithdrawFundsAsync(Withdrawal withdrawal)
        {
            decimal entryAmount = -1 * withdrawal.Amount;
            Balance currentBalance = await GetBalanceAsync();
            decimal currentBalanceAmount = currentBalance.Amount;

            WalletEntry withdrawalEntry = new WalletEntry()
            {
                Amount = entryAmount,
                BalanceBefore = currentBalanceAmount,
                EventTime = DateTimeOffset.UtcNow
            };

            await _walletRepository.InsertWalletEntryAsync(withdrawalEntry);

            Balance newBalance = new Balance
            {
                Amount = currentBalanceAmount + entryAmount
            };

            return newBalance;
        }
    }
}
