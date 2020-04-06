using Microsoft.EntityFrameworkCore;
using Pragmatest.Wallets.Data.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Pragmatest.Wallets.Data
{
    internal class WalletContext : DbContext
    {
        public DbSet<WalletEntry> Transactions { get; set; }

        public WalletContext(DbContextOptions<WalletContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new WalletEntryConfiguration());
        }
    }
}
