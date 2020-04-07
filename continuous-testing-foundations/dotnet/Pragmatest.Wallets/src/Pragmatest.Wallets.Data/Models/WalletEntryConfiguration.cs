using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pragmatest.Wallets.Data.Models
{
    internal class WalletEntryConfiguration : IEntityTypeConfiguration<WalletEntry>
    {
        public void Configure(EntityTypeBuilder<WalletEntry> builder)
        {   
            builder.HasKey(walletEntry => walletEntry.Id);

            builder.Property(ci => ci.Id)
                .IsRequired();
        }
    }
}
