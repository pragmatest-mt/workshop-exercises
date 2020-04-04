using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Services;
using Pragmatest.Wallets.Web.Controllers;
using Pragmatest.Wallets.Web.Mappers;
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
            // Arrange
            decimal expectedBalanceAmount = 4;

            Balance expectedBalance = new Balance() { Amount = expectedBalanceAmount };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = expectedBalanceAmount };

            ILogger<WalletController> logger = Mock.Of<ILogger<WalletController>>();

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            walletServiceMock
                .Setup(walletService => walletService.GetBalanceAsync())
                .Returns(Task.FromResult(expectedBalance));

            IWalletService walletService = walletServiceMock.Object;

            IMapper mapper = Mock.Of<IMapper>(mapper =>
                mapper.Map<BalanceResponse>(expectedBalance) == expectedBalanceResponse
            );

            WalletController walletController = new WalletController(logger, mapper, walletService);

            // Act 
            ActionResult<BalanceResponse> actionResult = await walletController.Balance();
            ActionResult actualActionResult = actionResult.Result;

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            BalanceResponse actualBalanceResponse = Assert.IsType<BalanceResponse>(okObjectResult.Value);

            Assert.Equal(expectedBalanceResponse, actualBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.GetBalanceAsync(), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Deposit_Deposit10_ReturnsOkAndBalance15()
        {
            // Arrange
            decimal expectedBalanceAmount = 15;
            decimal depositAmount = 10;


            Balance expectedBalance = new Balance() { Amount = expectedBalanceAmount };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = expectedBalanceAmount };

            ILogger<WalletController> logger = Mock.Of<ILogger<WalletController>>();

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Deposit deposit = new Deposit { Amount = depositAmount };

            walletServiceMock
                .Setup(walletService => walletService.DepositFundsAsync(deposit))
                .Returns(Task.FromResult(expectedBalance));

            IWalletService walletService = walletServiceMock.Object;

            WithdrawalRequest depositRequest = new WithdrawalRequest { Amount = depositAmount };

            IMapper mapper = Mock.Of<IMapper>(mapper => mapper.Map<Deposit>(depositRequest) == deposit
                                    && mapper.Map<BalanceResponse>(expectedBalance) == expectedBalanceResponse);

            WalletController walletController = new WalletController(logger, mapper, walletService);

            // Act
            ActionResult<BalanceResponse> actionResult = await walletController.Deposit(depositRequest);
            ActionResult actualActionResult = actionResult.Result;

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            BalanceResponse actualBalanceResponse = Assert.IsType<BalanceResponse>(okObjectResult.Value);

            Assert.Equal(expectedBalanceResponse, actualBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.DepositFundsAsync(deposit), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Deposit_DepositInvalidAmounts_ReturnsBadRequest(int depositAmount)
        {
            // Arrange
            ILogger<WalletController> logger = Mock.Of<ILogger<WalletController>>();

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Deposit deposit = new Deposit { Amount = depositAmount };

            IWalletService walletService = walletServiceMock.Object;

            WithdrawalRequest depositRequest = new WithdrawalRequest { Amount = depositAmount };

            IMapper mapper = Mock.Of<IMapper>(mapper => mapper.Map<Deposit>(depositRequest) == deposit);

            WalletController walletController = new WalletController(logger, mapper, walletService);

            // Act
            ActionResult<BalanceResponse> actionResult = await walletController.Deposit(depositRequest);
            ActionResult actualActionResult = actionResult.Result;

            // Assert
            BadRequestResult badRequestResult = Assert.IsType<BadRequestResult>(actionResult.Result);

            walletServiceMock.Verify(walletService => walletService.DepositFundsAsync(deposit), Times.Never);
            walletServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Withdrawal_Withdraw20_ReturnsOkAndBalance5()
        {
            // Arrange
            decimal expectedBalanceAmount = 5;
            decimal withdrawalAmount = 20;
            
            Balance expectedBalance = new Balance() { Amount = expectedBalanceAmount };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = expectedBalanceAmount };

            ILogger<WalletController> logger = Mock.Of<ILogger<WalletController>>();

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };

            walletServiceMock
                .Setup(walletService => walletService.WithdrawFundsAsync(withdrawal))
                .Returns(Task.FromResult(expectedBalance));

            IWalletService walletService = walletServiceMock.Object;

            WithdrawalRequest withdrawalRequest = new WithdrawalRequest { Amount = withdrawalAmount };

            IMapper mapper = Mock.Of<IMapper>(mapper => mapper.Map<Withdrawal>(withdrawalRequest) == withdrawal
                                    && mapper.Map<BalanceResponse>(expectedBalance) == expectedBalanceResponse);

            WalletController walletController = new WalletController(logger, mapper, walletService);

            // Act
            ActionResult<BalanceResponse> actionResult = await walletController.Withdraw(withdrawalRequest);
            ActionResult actualActionResult = actionResult.Result;

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            BalanceResponse actualBalanceResponse = Assert.IsType<BalanceResponse>(okObjectResult.Value);

            Assert.Equal(expectedBalanceResponse, actualBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.WithdrawFundsAsync(withdrawal), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }        
    }
}