using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Services;
using Pragmatest.Wallets.TestUtilities;
using Pragmatest.Wallets.Web.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
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
            string endpoint = "Wallet/Balance";

            decimal expectedBalanceAmount = 4;

            Balance expectedBalance = new Balance() { Amount = expectedBalanceAmount };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = expectedBalanceAmount };

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
            HttpResponseMessage response = await client.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            BalanceResponse actualBalanceResponse = await response.Content.ReadAsAsync<BalanceResponse>(new[] { new JsonMediaTypeFormatter() });
            actualBalanceResponse.ShouldCompare(expectedBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.GetBalanceAsync(), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Deposit_Deposit10_Returns20Async()
        {
            // Arrange
            string endpoint = "Wallet/Deposit";

            decimal expectedBalanceAmount = 20;
            decimal depositAmount = 10;

            Balance expectedBalance = new Balance() { Amount = expectedBalanceAmount };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = expectedBalanceAmount };

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Deposit deposit = new Deposit { Amount = depositAmount };

            walletServiceMock
             .Setup(walletService => walletService.DepositFundsAsync(It.Is<Deposit>(d => d.Equals(deposit))))
                 .Returns(Task.FromResult(expectedBalance));

            IWalletService walletService = walletServiceMock.Object;

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );

            DepositRequest depositRequest = new DepositRequest { Amount = depositAmount };

            StringContent content = new StringContent(JsonConvert.SerializeObject(depositRequest), Encoding.UTF8, "application/json");

            HttpClient client = factory.CreateClient();

            // Act  
            HttpResponseMessage response = await client.PostAsync(endpoint, content);


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            BalanceResponse actualBalanceResponse = await response.Content.ReadAsAsync<BalanceResponse>(new[] { new JsonMediaTypeFormatter() });
            actualBalanceResponse.ShouldCompare(expectedBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.DepositFundsAsync(deposit), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Deposit_DepositInvalidAmounts_ReturnsBadRequest(int depositAmount)
        {
            // Arrange
            string endpoint = "Wallet/Deposit";

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Deposit deposit = new Deposit { Amount = depositAmount };

            IWalletService walletService = walletServiceMock.Object;

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );

            DepositRequest depositRequest = new DepositRequest { Amount = depositAmount };

            StringContent content = new StringContent(JsonConvert.SerializeObject(depositRequest), Encoding.UTF8, "application/json");

            HttpClient client = factory.CreateClient();

            // Act  
            HttpResponseMessage response = await client.PostAsync(endpoint, content);


            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            walletServiceMock.Verify(walletService => walletService.DepositFundsAsync(deposit), Times.Never);
            walletServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Withdraw_Withdraw5_Returns10Async()
        {
            // Arrange
            string endpoint = "Wallet/Withdraw";

            decimal expectedBalanceAmount = 10;
            decimal withdrawalAmount = 5;

            Balance expectedBalance = new Balance() { Amount = expectedBalanceAmount };
            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = expectedBalanceAmount };

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };

            walletServiceMock
             .Setup(walletService => walletService.WithdrawFundsAsync(It.Is<Withdrawal>(d => d.Equals(withdrawal))))
                 .Returns(Task.FromResult(expectedBalance));

            IWalletService walletService = walletServiceMock.Object;

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );

            WithdrawalRequest withdrawalRequest = new WithdrawalRequest { Amount = withdrawalAmount };

            StringContent content = new StringContent(JsonConvert.SerializeObject(withdrawalRequest), Encoding.UTF8, "application/json");

            HttpClient client = factory.CreateClient();

            // Act  
            HttpResponseMessage response = await client.PostAsync(endpoint, content);


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            BalanceResponse actualBalanceResponse = await response.Content.ReadAsAsync<BalanceResponse>(new[] { new JsonMediaTypeFormatter() });
            actualBalanceResponse.ShouldCompare(expectedBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.WithdrawFundsAsync(withdrawal), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }
    }
}