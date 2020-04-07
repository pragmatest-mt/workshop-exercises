using KellermanSoftware.CompareNetObjects;
using Moq;
using Pragmatest.Wallets.Data.Models;
using Pragmatest.Wallets.Data.Repositories;
using Pragmatest.Wallets.Exceptions;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Services;
using System.Collections.Generic;
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
            decimal lastTransactionBalanceBefore = 1000;
            decimal lastTransactionAmount = 5000;
            decimal expectedBalanceBefore = 6000;
            decimal depositAmount = 500;
            decimal expectedBalanceAmount = 6500;

            Balance expectedBalance = new Balance { Amount = expectedBalanceAmount };

            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalanceBefore };
            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            WalletEntry newTransaction = new WalletEntry { Amount = depositAmount, BalanceBefore = expectedBalanceBefore };
            ICompareLogic compareLogic = new CompareLogic(new ComparisonConfig() { MembersToInclude = new List<string> {nameof(newTransaction.Amount), nameof(newTransaction.BalanceBefore)}});

            walletRepositoryMock
                .Setup(walletRepository => walletRepository.InsertWalletEntryAsync(It.Is<WalletEntry>(actualTransaction => compareLogic.Compare(newTransaction, actualTransaction).AreEqual)))
                .Returns(Task.CompletedTask);

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            IWalletService walletService = new WalletService(walletRepository);

            Deposit deposit = new Deposit { Amount = depositAmount };

            // Act
            Balance actualBalance = await walletService.DepositFundsAsync(deposit);

            // Assert
            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
            walletRepositoryMock.Verify(walletRepository => walletRepository.InsertWalletEntryAsync(It.Is<WalletEntry>(actualTransaction => compareLogic.Compare(newTransaction, actualTransaction).AreEqual)), Times.Once);
            walletRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DepositFundsAsync_UnderLimitDepositAmount_ThrowsLimitException()
        {
            //Arrange
            decimal depositAmount = 299;

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            IWalletRepository walletRepository = walletRepositoryMock.Object;

            IWalletService walletService = new WalletService(walletRepository);

            Deposit deposit = new Deposit { Amount = depositAmount };

            // Act
            async Task depositTask() => await walletService.DepositFundsAsync(deposit);

            // Assert
            await Assert.ThrowsAsync<LimitException>(depositTask);

            walletRepositoryMock.VerifyNoOtherCalls();
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
