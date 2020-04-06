using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Pragmatest.Wallets.Data.Models;
using Pragmatest.Wallets.Data.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pragmatest.Wallets.Data.IntegrationTests
{
    public class WalletRepositoryIntegrationTests
    {
        [Fact]
        public async Task GetLastWalletEntryAsync_NoEntries_ReturnsNull()
        {
            // Arrange 
            DbContextOptions<WalletContext> dbContextOptions = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: "GetLastWalletEntryAsync_NoEntries_ReturnsNull")
                .Options;

            // Act
            WalletEntry walletEntry;
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                IWalletRepository walletRepository = new WalletRepository(context);
                walletEntry = await walletRepository.GetLastWalletEntryAsync();
            }

            // Assert
            Assert.Null(walletEntry);
        }

        [Fact]
        public async Task GetLastWalletEntryAsync_SingleEntry_ReturnsTheEntry()
        {
            // Arrange 
            DbContextOptions<WalletContext> dbContextOptions = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: "GetLastWalletEntryAsync_SingleEntry_ReturnsTheEntry")
                .Options;
            
            WalletEntry expectedEntry = new WalletEntry { Id = Guid.NewGuid().ToString(), EventTime = DateTime.UtcNow, Amount = 10, BalanceBefore = 0 };
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                context.Add(expectedEntry);
                context.SaveChanges();
            }

            // Act
            WalletEntry actualEntry;
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                IWalletRepository walletRepository = new WalletRepository(context);
                actualEntry = await walletRepository.GetLastWalletEntryAsync();
            }

            // Assert
            Assert.NotNull(actualEntry);
            actualEntry.ShouldCompare(expectedEntry);
        }

        [Fact]
        public async Task GetLastWalletEntryAsync_MultipleEntries_ReturnsTheMostRecentEntry()
        {
            // Arrange 
            DbContextOptions<WalletContext> dbContextOptions = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: "GetLastWalletEntryAsync_MultipleEntries_ReturnsTheMostRecentEntry")
                .Options;

            WalletEntry previousEntry = new WalletEntry { Id = Guid.NewGuid().ToString(), EventTime = DateTime.UtcNow, Amount = 10, BalanceBefore = 0 };
            WalletEntry expectedEntry = new WalletEntry { Id = Guid.NewGuid().ToString(), EventTime = DateTime.UtcNow.AddTicks(1), Amount = 10, BalanceBefore = 0 };
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                context.Add(expectedEntry);
                context.Add(previousEntry);
                context.SaveChanges();
            }

            // Act
            WalletEntry actualEntry;
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                IWalletRepository walletRepository = new WalletRepository(context);
                actualEntry = await walletRepository.GetLastWalletEntryAsync();
            }

            // Assert
            Assert.NotNull(actualEntry);
            actualEntry.ShouldCompare(expectedEntry);
        }

        [Fact]
        public async Task InsertWalletAsync_NoEntries_SingleWalletEntry()
        {
            // Arrange 
            DbContextOptions<WalletContext> dbContextOptions = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: "InsertWalletAsync_NoEntries_SingleWalletEntry")
                .Options;

            // Act
            WalletEntry expectedEntry = new WalletEntry { Id = Guid.NewGuid().ToString(), EventTime = DateTime.UtcNow, Amount = 10, BalanceBefore = 0 };
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                IWalletRepository walletRepository = new WalletRepository(context);
                await walletRepository.InsertWalletEntryAsync(expectedEntry);
            }

            // Assert
            DbSet<WalletEntry> actualWalletEntries;
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                actualWalletEntries = context.Transactions;
                Assert.Collection(actualWalletEntries, actualWalletEntry => actualWalletEntry.ShouldCompare(expectedEntry));
            }
        }

        [Fact]
        public async Task InsertWalletAsync_PreviousWalletEntries_NewWalletEntry()
        {
            // Arrange 
            DbContextOptions<WalletContext> dbContextOptions = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: "InsertWalletAsync_PreviousWalletEntries_NewWalletEntry")
                .Options;

            WalletEntry[] previousEntries = new[] {
                new WalletEntry{ Id = Guid.NewGuid().ToString(), EventTime = DateTime.UtcNow, Amount = 10, BalanceBefore = 0 },
                new WalletEntry { Id = Guid.NewGuid().ToString(), EventTime = DateTime.UtcNow.AddTicks(1), Amount = 10, BalanceBefore = 0 }
            };
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                await context.AddRangeAsync(previousEntries); 
                context.SaveChanges();
            }

            // Act
            WalletEntry expectedNewEntry = new WalletEntry { Id = Guid.NewGuid().ToString(), EventTime = DateTime.UtcNow, Amount = 10, BalanceBefore = 0 };
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                IWalletRepository walletRepository = new WalletRepository(context);
                await walletRepository.InsertWalletEntryAsync(expectedNewEntry);
            }

            // Assert
            DbSet<WalletEntry> actualWalletEntries;
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                actualWalletEntries = context.Transactions;
                Assert.Collection(actualWalletEntries, 
                    walletEntry => walletEntry.ShouldCompare(previousEntries[0]),
                    walletEntry => walletEntry.ShouldCompare(previousEntries[1]),
                    walletEntry => walletEntry.ShouldCompare(expectedNewEntry));
            }
        }

        [Fact]
        public async Task InsertWalletAsync_MissingId_ThrowsInvalidOperationException()
        {
            // Arrange 
            DbContextOptions<WalletContext> dbContextOptions = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: "InsertWalletAsync_MissingId_ThrowsInvalidOperationException")
                .Options;

            // Act / Assert
            WalletEntry missingIdEntry = new WalletEntry { Id = null, EventTime = DateTime.UtcNow, Amount = 10, BalanceBefore = 0 };
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                IWalletRepository walletRepository = new WalletRepository(context);
                await Assert.ThrowsAsync<InvalidOperationException>(() => 
                    walletRepository.InsertWalletEntryAsync(missingIdEntry)
                );
            }
        }

        [Fact]
        public async Task InsertWalletAsync_DuplicateId_ThrowsArgumentException()
        {
            // Arrange 
            DbContextOptions<WalletContext> dbContextOptions = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: "InsertWalletAsync_DuplicateId_ThrowsArgumentException")
                .Options;

            WalletEntry originalWalletEntry = 
                new WalletEntry{ Id = "IAmDuplicate", EventTime = DateTime.UtcNow, Amount = 10, BalanceBefore = 0 };

            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                await context.AddAsync(originalWalletEntry);
                context.SaveChanges();
            }

            // Act / Assert
            WalletEntry duplicateEntry = new WalletEntry { Id = "IAmDuplicate", EventTime = DateTime.UtcNow, Amount = 10, BalanceBefore = 0 };
            using (WalletContext context = new WalletContext(dbContextOptions))
            {
                IWalletRepository walletRepository = new WalletRepository(context);
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    walletRepository.InsertWalletEntryAsync(duplicateEntry)
                );
            }
        }
    }
}
