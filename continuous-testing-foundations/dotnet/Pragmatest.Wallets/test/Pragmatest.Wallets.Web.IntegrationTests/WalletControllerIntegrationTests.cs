using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Services;
using Pragmatest.Wallets.TestUtilities;
using Pragmatest.Wallets.Web.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;

namespace Pragmatest.Wallets.Web.IntegrationTests
{
    public class WalletControllerIntegrationTests
    {
        [Fact]
        public async Task GetBalanceAsync_CurrentBalanceIs4_Returns4Async()
        {
            // Arrange
            Balance expectedBalance = new Balance() { Amount = 4 };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = 4 };

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            walletServiceMock
                .Setup(walletService => walletService.GetBalanceAsync())
                .Returns(Task.FromResult(expectedBalance));

            IWalletService walletService = walletServiceMock.Object;

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );

            HttpClient client = factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("Wallet/Balance");

            

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            BalanceResponse actualBalanceResponse = await response.Content.ReadAsAsync<BalanceResponse>(new[] { new JsonMediaTypeFormatter() });
            actualBalanceResponse.ShouldCompare(expectedBalanceResponse);
            
            walletServiceMock.Verify(walletService => walletService.GetBalanceAsync(), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }
    }
}
