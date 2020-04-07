using Microsoft.Extensions.DependencyInjection;
using Pragmatest.Wallets.Services;

namespace Pragmatest.Wallets.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterWalletService(this IServiceCollection services)
        {
            return services.AddScoped<IWalletService, WalletService>();
        }
    }
}
