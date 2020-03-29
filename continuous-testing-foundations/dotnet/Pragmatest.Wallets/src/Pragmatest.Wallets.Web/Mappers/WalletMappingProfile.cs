using AutoMapper;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Web.Models;

namespace Pragmatest.Wallets.Web.Mappers
{
    public class WalletMappingProfile : Profile
    {
        public WalletMappingProfile()
        {
            CreateMap<Balance, BalanceResponse>();
            CreateMap<DepositRequest, Deposit>();
            CreateMap<WithdrawalRequest, Withdrawal>();
        }
    }
}
