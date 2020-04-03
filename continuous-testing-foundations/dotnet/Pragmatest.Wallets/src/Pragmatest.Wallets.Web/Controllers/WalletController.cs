using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Services;
using Pragmatest.Wallets.Web.Models;
using System.Threading.Tasks;

namespace Pragmatest.Wallets.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IMapper _mapper;
        private readonly IWalletService _walletService;

        public WalletController(ILogger<WalletController> logger, IMapper mapper, IWalletService walletService)
        {
            _logger = logger;
            _mapper = mapper;
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<ActionResult<BalanceResponse>> Balance()
        {
            Balance balance = await _walletService.GetBalanceAsync();

            BalanceResponse balanceResponse = _mapper.Map<BalanceResponse>(balance);
            
            return Ok(balanceResponse);
        }

        [HttpPost]
        public async Task<ActionResult<BalanceResponse>> Deposit(DepositRequest depositRequest)
        {
            Deposit deposit = _mapper.Map<Deposit>(depositRequest);

            Balance balance =  await _walletService.DepositFundsAsync(deposit);

            BalanceResponse balanceResponse = _mapper.Map<BalanceResponse>(balance);

            return Ok(balanceResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawalRequest withdrawalRequest)
        {
            Withdrawal withdrawal = _mapper.Map<Withdrawal>(withdrawalRequest); 

            Balance balance = await _walletService.WithdrawFundsAsync(withdrawal);

            BalanceResponse balanceResponse = _mapper.Map<BalanceResponse>(balance);

            return Ok(balanceResponse);
        }


    }
}
