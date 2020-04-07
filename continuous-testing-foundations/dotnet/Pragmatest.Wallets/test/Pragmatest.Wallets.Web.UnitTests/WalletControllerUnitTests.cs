using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Services;
using Pragmatest.Wallets.Web.Controllers;
using Pragmatest.Wallets.Web.Models;
using System.Threading.Tasks;
using Xunit;

namespace Pragmatest.Wallets.Web.UnitTests
{
    public class WalletControllerUnitTests
    {
        [Fact]
        public async Task Balance_GetBalanceIs4_ReturnsOkAndBalance4()
        {
            //// Arrange

            decimal currentBalanceAmount = 4;
            Balance currentBalance = new Balance() { Amount = currentBalanceAmount };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = currentBalanceAmount };

            // Setup Mocks

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            walletServiceMock
                .Setup(walletService => walletService.GetBalanceAsync())
                .Returns(Task.FromResult(currentBalance));

            IWalletService walletService = walletServiceMock.Object;

            IMapper mapper = Mock.Of<IMapper>(mapper =>
                mapper.Map<BalanceResponse>(currentBalance) == expectedBalanceResponse
            );

            ILogger<WalletController> logger = Mock.Of<ILogger<WalletController>>();

            // Initialize SUT            

            WalletController walletController = new WalletController(logger, mapper, walletService);

            //// Act 
            
            ActionResult<BalanceResponse> actionResult = await walletController.Balance();
            ActionResult actualActionResult = actionResult.Result;

            /// Assert
            
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            BalanceResponse actualBalanceResponse = Assert.IsType<BalanceResponse>(okObjectResult.Value);

            Assert.Equal(expectedBalanceResponse, actualBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.GetBalanceAsync(), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Deposit_Deposit10_ReturnsOkAndPostDepositBalance()
        {
            //// Arrange
            
            decimal postDepositBalance = 15;
            Balance expectedBalance = new Balance() { Amount = postDepositBalance };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = postDepositBalance };

            decimal depositAmount = 10;
            DepositRequest depositRequest = new DepositRequest { Amount = depositAmount };

            // Setup Mocks

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Deposit deposit = new Deposit { Amount = depositAmount };

            walletServiceMock
                .Setup(walletService => walletService.DepositFundsAsync(deposit))
                .Returns(Task.FromResult(expectedBalance));

            IWalletService walletService = walletServiceMock.Object;

            ILogger<WalletController> logger = Mock.Of<ILogger<WalletController>>();

            IMapper mapper = Mock.Of<IMapper>(mapper => mapper.Map<Deposit>(depositRequest) == deposit
                                    && mapper.Map<BalanceResponse>(expectedBalance) == expectedBalanceResponse);
            
            // Initialize SUT

            WalletController walletController = new WalletController(logger, mapper, walletService);

            //// Act
            
            ActionResult<BalanceResponse> actionResult = await walletController.Deposit(depositRequest);
            ActionResult actualActionResult = actionResult.Result;

            //// Assert
            
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            BalanceResponse actualBalanceResponse = Assert.IsType<BalanceResponse>(okObjectResult.Value);

            Assert.Equal(expectedBalanceResponse, actualBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.DepositFundsAsync(deposit), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task Withdrawal_Withdraw20_ReturnsOkAndBalance5()
        {
            //// Arrange
            
            decimal postWithdrawalBalanceAmount = 5;
            Balance postWithdrawalBalance = new Balance() { Amount = postWithdrawalBalanceAmount };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = postWithdrawalBalanceAmount };

            decimal withdrawalAmount = 20;
            WithdrawalRequest withdrawalRequest = new WithdrawalRequest { Amount = withdrawalAmount };

            // Setup Mocks

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };

            walletServiceMock
                .Setup(walletService => walletService.WithdrawFundsAsync(withdrawal))
                .Returns(Task.FromResult(postWithdrawalBalance));

            IWalletService walletService = walletServiceMock.Object;

            ILogger<WalletController> logger = Mock.Of<ILogger<WalletController>>();

            IMapper mapper = Mock.Of<IMapper>(mapper => mapper.Map<Withdrawal>(withdrawalRequest) == withdrawal
                                    && mapper.Map<BalanceResponse>(postWithdrawalBalance) == expectedBalanceResponse);

            // Initialize SUT 

            WalletController walletController = new WalletController(logger, mapper, walletService);

            //// Act
            
            ActionResult<BalanceResponse> actionResult = await walletController.Withdraw(withdrawalRequest);
            ActionResult actualActionResult = actionResult.Result;

            //// Assert
            
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            BalanceResponse actualBalanceResponse = Assert.IsType<BalanceResponse>(okObjectResult.Value);

            Assert.Equal(expectedBalanceResponse, actualBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.WithdrawFundsAsync(withdrawal), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }        
    }
}