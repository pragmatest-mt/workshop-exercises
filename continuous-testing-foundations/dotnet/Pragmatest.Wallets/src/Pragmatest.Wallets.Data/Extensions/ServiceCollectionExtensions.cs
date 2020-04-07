using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pragmatest.Wallets.Data.Repositories;

namespace Pragmatest.Wallets.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterWalletRepository(this IServiceCollection services)
        {
            services.AddDbContext<WalletContext>(context => context.UseInMemoryDatabase("Wallet"));
            services.AddScoped<IWalletRepository, WalletRepository>();
            return services;
        }
    }
}
