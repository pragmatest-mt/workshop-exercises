using KellermanSoftware.CompareNetObjects;
using Moq;
using Pragmatest.Wallets.Data.Models;
using Pragmatest.Wallets.Data.Repositories;
using Pragmatest.Wallets.Exceptions;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Services;
using System.Threading.Tasks;
using Xunit;

namespace Pragmatest.Wallets.UnitTests
{
    public class WalletServiceUnitTests
    {
        [Fact]
        public async Task GetBalanceAsync_NoPreviousTransactions_Returns0()
        {
            // Arrange
            Balance expectedBalance = new Balance { Amount = 0 };

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(default(WalletEntry)));

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            IWalletService walletService = new WalletService(walletRepository);

            // Act
            Balance actualBalance = await walletService.GetBalanceAsync();

            // Assert
            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
        }

        [Theory]
        [InlineData(5, 10, 15)]
        [InlineData(5, -2, 3)]
        public async Task GetBalanceAsync_ExistingLastTransaction_ReturnsExpectedBalance(decimal lastTransactionBalance, decimal lastTransactionAmount, decimal expectedBalanceAmont)
        {
            // Arrange
            Balance expectedBalance = new Balance { Amount = expectedBalanceAmont };

            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalance };

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            IWalletService walletService = new WalletService(walletRepository);

            // Act
            Balance actualBalance = await walletService.GetBalanceAsync();

            // Assert
            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
        }

        [Fact]
        public async Task DepositFundsAsync_ValidDepositAmount_ReturnsExpectedBalance()
        {
            //Arrange
            decimal lastTransactionBalance = 10;
            decimal lastTransactionAmount = 5;
            decimal depositAmount = 5;
            decimal expectedBalanceAmount = 20;

            Balance expectedBalance = new Balance { Amount = expectedBalanceAmount };

            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalance };

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            IWalletService walletService = new WalletService(walletRepository);

            Deposit deposit = new Deposit { Amount = depositAmount };

            // Act
            Balance actualBalance = await walletService.DepositFundsAsync(deposit);

            // Assert
            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
        }

        [Fact]
        public async Task WithdrawFundsAsync_ValidWithdrawalAmount_ReturnsExpectedBalance()
        {
            //Arrange
            decimal lastTransactionBalance = 20;
            decimal lastTransactionAmount = 10;
            decimal withdrawalAmount = 15;
            decimal expectedBalanceAmount = 15;

            Balance expectedBalance = new Balance { Amount = expectedBalanceAmount };

            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalance };

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            IWalletService walletService = new WalletService(walletRepository);

            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };

            // Act
            Balance actualBalance = await walletService.WithdrawFundsAsync(withdrawal);

            // Assert
            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
        }

        [Fact]
        public async Task WithdrawalFundsAsync_WithdrawalAmountExceedsBalance_ThrowsInsufficientBalanceException()
        {
            //Arrange
            decimal lastTransactionBalance = 50;
            decimal lastTransactionAmount = 10;
            decimal withdrawalAmount = 100;


            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalance };

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            IWalletService walletService = new WalletService(walletRepository);

            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };

            // Act
            async Task withdrawalTask() => await walletService.WithdrawFundsAsync(withdrawal);

            // Assert
            await Assert.ThrowsAsync<InsufficientBalanceException>(withdrawalTask);


            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
        }
    }
}
