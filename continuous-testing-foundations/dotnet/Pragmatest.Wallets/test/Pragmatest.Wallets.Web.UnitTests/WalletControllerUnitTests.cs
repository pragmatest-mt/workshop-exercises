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
            // Arrange 
            Balance expectedBalance = new Balance() { Amount = 4 };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = 4 };

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
    }
}