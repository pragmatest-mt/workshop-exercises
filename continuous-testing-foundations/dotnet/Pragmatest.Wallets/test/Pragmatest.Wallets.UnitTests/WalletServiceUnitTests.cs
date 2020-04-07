using KellermanSoftware.CompareNetObjects;
using Moq;
using Pragmatest.Wallets.Data.Models;
using Pragmatest.Wallets.Data.Repositories;
using Pragmatest.Wallets.Exceptions;
using Pragmatest.Wallets.Models;
using Pragmatest.Wallets.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pragmatest.Wallets.UnitTests
{
    public class WalletServiceUnitTests
    {
        private static readonly Func<WalletEntry, decimal, decimal, bool> _compareWalletEntry = (walletEntry, amount, balanceBefore) =>
                          walletEntry.Amount == amount &&
                          walletEntry.BalanceBefore == balanceBefore;

       [Fact]
        public async Task GetBalanceAsync_NoPreviousTransactions_Returns0()
        {
            //// Arrange

            // Setup Mocks

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(default(WalletEntry)));

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            // Initialize SUT

            IWalletService walletService = new WalletService(walletRepository);

            // Set Expectations

            Balance expectedBalance = new Balance { Amount = 0 };

            //// Act

            Balance actualBalance = await walletService.GetBalanceAsync();

            //// Assert

            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
        }

        [Theory]
        [InlineData(5, 10, 15)]
        [InlineData(5, -2, 3)]
        public async Task GetBalanceAsync_ExistingLastTransaction_ReturnsExpectedBalance(decimal lastTransactionBalanceBefore, decimal lastTransactionAmount, decimal expectedBalanceAmont)
        {
            //// Arrange

            // Setup Mocks

            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalanceBefore };

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            // Initialize SUT

            IWalletService walletService = new WalletService(walletRepository);

            // Set Expectations

            Balance expectedBalance = new Balance { Amount = expectedBalanceAmont };

            //// Act

            Balance actualBalance = await walletService.GetBalanceAsync();

            //// Assert

            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
        }

        [Fact]
        public async Task DepositFundsAsync_ValidDepositAmount_ReturnsExpectedBalance()
        {
            //// Arrange

            decimal lastTransactionBalanceBefore = 10;
            decimal lastTransactionAmount = 5;
            decimal depositAmount = 5;
            decimal depositBalanceBefore = 15;

            // Setup Mocks

            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();

            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalanceBefore };
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            walletRepositoryMock
                .Setup(walletRepositoryMock => walletRepositoryMock.InsertWalletEntryAsync(It.Is<WalletEntry>(
                        walletEntry => _compareWalletEntry(walletEntry, depositAmount, depositBalanceBefore)
                    ))
                )
                .Returns(Task.CompletedTask);

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            // Initialize SUT

            IWalletService walletService = new WalletService(walletRepository);
            Deposit deposit = new Deposit { Amount = depositAmount };

            // Set expectations

            decimal expectedBalanceAmount = 20;
            Balance expectedBalance = new Balance { Amount = expectedBalanceAmount };

            //// Act

            Balance actualBalance = await walletService.DepositFundsAsync(deposit);

            //// Assert

            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
            walletRepositoryMock.Verify(walletRepository => walletRepository.InsertWalletEntryAsync(It.Is<WalletEntry>(
                    walletEntry => _compareWalletEntry(walletEntry, depositAmount, depositBalanceBefore))
                ), Times.Once);
            walletRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task WithdrawFundsAsync_ValidWithdrawalAmount_ReturnsExpectedBalance()
        {
            ////Arrange
            
            decimal lastTransactionBalanceBefore = 20;
            decimal lastTransactionAmount = 10;
            decimal withdrawalAmount = 15;
            decimal withdrawalBalanceBefore = 30;

            // Setup Mocks
           
            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();

            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalanceBefore };
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            walletRepositoryMock
                .Setup(walletRepositoryMock => walletRepositoryMock.InsertWalletEntryAsync(It.Is<WalletEntry>(
                    walletEntry => _compareWalletEntry(walletEntry, withdrawalAmount, withdrawalBalanceBefore)))
                )
                .Returns(Task.CompletedTask);

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            // Initialize SUT 

            IWalletService walletService = new WalletService(walletRepository);
            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };
            
            // Set expectations 

            decimal expectedBalanceAmount = 15;
            Balance expectedBalance = new Balance { Amount = expectedBalanceAmount };


            //// Act
            
            Balance actualBalance = await walletService.WithdrawFundsAsync(withdrawal);

            //// Assert
            
            actualBalance.ShouldCompare(expectedBalance);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
            walletRepositoryMock.Verify(walletRepository => walletRepository.InsertWalletEntryAsync(It.Is<WalletEntry>(
                    walletEntry => _compareWalletEntry(walletEntry, -1 * withdrawalAmount, withdrawalBalanceBefore))
                ), Times.Once);
            walletRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task WithdrawalFundsAsync_WithdrawalAmountExceedsBalance_ThrowsInsufficientBalanceException()
        {
            ////Arrange
            
            decimal lastTransactionBalanceBefore = 50;
            decimal lastTransactionAmount = 10;
            decimal withdrawalAmount = 100;

            // Setup Mocks 

            WalletEntry lastTransaction = new WalletEntry { Amount = lastTransactionAmount, BalanceBefore = lastTransactionBalanceBefore };
            Mock<IWalletRepository> walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock
                .Setup(walletRepository => walletRepository.GetLastWalletEntryAsync())
                .Returns(Task.FromResult(lastTransaction));

            IWalletRepository walletRepository = walletRepositoryMock.Object;

            // Initialize SUT 
             
            IWalletService walletService = new WalletService(walletRepository);
            Withdrawal withdrawal = new Withdrawal { Amount = withdrawalAmount };

            //// Act
            
            async Task withdrawalTask() => await walletService.WithdrawFundsAsync(withdrawal);

            //// Assert
            
            await Assert.ThrowsAsync<InsufficientBalanceException>(withdrawalTask);

            walletRepositoryMock.Verify(walletRepository => walletRepository.GetLastWalletEntryAsync(), Times.Once);
            walletRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
