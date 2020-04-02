using Microsoft.Extensions.DependencyInjection;
using Pragmatest.Wallets.Data.Repositories;

namespace Pragmatest.Wallets.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterWalletRepository(this IServiceCollection services)
        {
            return services.AddSingleton<IWalletRepository, WalletRepository>();
        }
    }
}
