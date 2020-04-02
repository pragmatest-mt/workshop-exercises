using KellermanSoftware.CompareNetObjects;
using Moq;
using Pragmatest.Wallets.Data.Models;
using Pragmatest.Wallets.Data.Repositories;
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
        public async Task GetBalanceAsync_ExistingLastTransaction_ReturnsExpectedAmount(decimal lastTransactionBalance, decimal lastTransactionAmount, decimal expectedBalanceAmont)
        {
            // Arrange
            Balance expectedBalance = new Balance { Amount = expectedBalanceAmont };
            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionBalance, BalanceBefore = lastTransactionAmount };

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
    }
}
