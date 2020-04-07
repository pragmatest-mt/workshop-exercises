using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Pragmatest.Wallets.Exceptions;
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
        private static readonly CompareLogic _comparer = new CompareLogic();

        [Fact]
        public async Task GetBalanceAsync_CurrentBalanceIs4_ReturnsOk4Async()
        {
            //// Arrange

            decimal currentBalanceAmount = 4;
            
            // Setup Mocks

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();
            
            Balance currentBalance = new Balance() { Amount = currentBalanceAmount };
            walletServiceMock
                .Setup(walletService => walletService.GetBalanceAsync())
                .Returns(Task.FromResult(currentBalance));

            IWalletService walletService = walletServiceMock.Object;

            // Initialize HTTP client and request data

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );
            HttpClient client = factory.CreateClient();

            string endpoint = "Wallet/Balance";

            // Set Expectations

            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = currentBalanceAmount };

            //// Act

            HttpResponseMessage response = await client.GetAsync(endpoint);
            BalanceResponse actualBalanceResponse = await response.Content.ReadAsAsync<BalanceResponse>(new[] { new JsonMediaTypeFormatter() });

            //// Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            actualBalanceResponse.ShouldCompare(expectedBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.GetBalanceAsync(), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Deposit_Deposit10_ReturnsOkPostDepositBalanceAsync()
        {
            //// Arrange
            
            decimal postDepositBalanceAmount = 20;
            decimal depositAmount = 10;

            // Setup Mocks 

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Balance postDepositBalance = new Balance() { Amount = postDepositBalanceAmount };
            Deposit deposit = new Deposit { Amount = depositAmount };
            
            walletServiceMock
             .Setup(walletService => 
                walletService.DepositFundsAsync(It.Is<Deposit>(actualDeposit => _comparer.Compare(deposit, actualDeposit).AreEqual))
             )
             .Returns(Task.FromResult(postDepositBalance));

            IWalletService walletService = walletServiceMock.Object;

            // Initialize HTTP client and request data

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );
            HttpClient client = factory.CreateClient();

            string endpoint = "Wallet/Deposit";
            DepositRequest depositRequest = new DepositRequest { Amount = depositAmount };
            StringContent payload = depositRequest.AsStringContent();
            
            // Set Expectations

            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = postDepositBalanceAmount };

            //// Act  

            HttpResponseMessage response = await client.PostAsync(endpoint, payload);
            BalanceResponse actualBalanceResponse = await response.Content.ReadAsAsync<BalanceResponse>(new[] { new JsonMediaTypeFormatter() });

            //// Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            actualBalanceResponse.ShouldCompare(expectedBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.DepositFundsAsync(It.Is<Deposit>(
                    actualDeposit => _comparer.Compare(deposit, actualDeposit).AreEqual)
                ), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        public async Task Deposit_DepositInvalidAmounts_ReturnsBadRequest(int depositAmount)
        {
            //// Arrange
            
            // Setup Mocks
            
            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();
            IWalletService walletService = walletServiceMock.Object;

            // Initialize HTTP client and request data

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );

            HttpClient client = factory.CreateClient();

            string endpoint = "Wallet/Deposit";
            DepositRequest depositRequest = new DepositRequest { Amount = depositAmount };
            StringContent content = depositRequest.AsStringContent();

            //// Act  

            HttpResponseMessage response = await client.PostAsync(endpoint, content);

            //// Assert
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            walletServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Withdraw_Withdraw5_ReturnsOkPostWithdrawalBalanceAsync()
        {
            //// Arrange
            
            decimal withdrawalAmount = 5;
            decimal postWithdrawalBalanceAmount = 10;

            // Setup Mocks

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };
            Balance postWithdrawalBalance = new Balance() { Amount = postWithdrawalBalanceAmount };
            walletServiceMock
                .Setup(walletService => 
                    walletService.WithdrawFundsAsync(It.Is<Withdrawal>(actualWithdrawal => _comparer.Compare(withdrawal, actualWithdrawal).AreEqual)))
                .Returns(Task.FromResult(postWithdrawalBalance));

            IWalletService walletService = walletServiceMock.Object;

            // Initialize HTTP client and request data

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );

            HttpClient client = factory.CreateClient();

            string endpoint = "Wallet/Withdraw";
            WithdrawalRequest withdrawalRequest = new WithdrawalRequest { Amount = withdrawalAmount };
            StringContent content = withdrawalRequest.AsStringContent();

            // Set Expectations

            BalanceResponse expectedBalanceResponse = new BalanceResponse() { Amount = postWithdrawalBalanceAmount };

            //// Act  

            HttpResponseMessage response = await client.PostAsync(endpoint, content);
            BalanceResponse actualBalanceResponse = await response.Content.ReadAsAsync<BalanceResponse>(new[] { new JsonMediaTypeFormatter() });

            //// Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            actualBalanceResponse.ShouldCompare(expectedBalanceResponse);

            walletServiceMock.Verify(walletService => walletService.WithdrawFundsAsync(It.Is<Withdrawal>(
                    actualWithdrawal => _comparer.Compare(withdrawal, actualWithdrawal).AreEqual)
                ), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        public async Task Withdrawal_WithdrawalInvalidAmounts_ReturnsBadRequest(int withdrawalAmount)
        {
            //// Arrange            
            
            // Setup Mocks 

            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();
            IWalletService walletService = walletServiceMock.Object;

            // Initialize HTTP client and request data
            
            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );

            HttpClient client = factory.CreateClient();

            string endpoint = "Wallet/Withdraw";
            WithdrawalRequest withdrawalRequest = new WithdrawalRequest { Amount = withdrawalAmount };
            StringContent content = withdrawalRequest.AsStringContent();

            //// Act  

            HttpResponseMessage response = await client.PostAsync(endpoint, content);

            //// Assert
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            
            walletServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Withdrawal_WithdrawalInsufficientBalanceException_ReturnsBadRequest()
        {
            //// Arrange
            
            int withdrawalAmount = 1000;
            
            // Setup Mocks
                        
            Mock<IWalletService> walletServiceMock = new Mock<IWalletService>();

            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };
            walletServiceMock
             .Setup(walletService => walletService.WithdrawFundsAsync(It.Is<Withdrawal>(
                 actualWithdrawal => _comparer.Compare(withdrawal, actualWithdrawal).AreEqual)))
             .Throws<InsufficientBalanceException>();

            IWalletService walletService = walletServiceMock.Object;

            // Initialize HTTP client and request data

            WebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>(services =>
                services.SwapTransient(provider => walletService)
            );

            HttpClient client = factory.CreateClient();

            string endpoint = "Wallet/Withdraw";
            WithdrawalRequest withdrawalRequest = new WithdrawalRequest { Amount = withdrawalAmount };
            StringContent content = withdrawalRequest.AsStringContent();

            //// Act  

            HttpResponseMessage response = await client.PostAsync(endpoint, content);

            //// Assert
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            
            walletServiceMock.Verify(walletService => walletService.WithdrawFundsAsync(It.Is<Withdrawal>(
                    actualWithdrawal => _comparer.Compare(withdrawal, actualWithdrawal).AreEqual)
                ), Times.Once);
            walletServiceMock.VerifyNoOtherCalls();
        }
    }
}
