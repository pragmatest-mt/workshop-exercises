using Pragmatest.Wallets.Models;
using System.Threading.Tasks;

namespace Pragmatest.Wallets.Services
{
    public interface IWalletService
    {
        Task<Balance> GetBalanceAsync();

        Task<Balance> DepositFundsAsync(Deposit deposit);

        Task<Balance> WithdrawFundsAsync(Withdrawal withdrawal);
    }
}
