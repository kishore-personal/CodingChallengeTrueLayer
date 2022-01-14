using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TLPokemon.Application.Interfaces.Services;
using TLPokemon.Infrastructure.Shared.Services;

namespace TLPokemon.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.AddSingleton(typeof(IPokemonService), typeof(PokemonService));
            services.AddSingleton(typeof(IShakespeareTranslationService), typeof(ShakespeareTranslationService));
            services.AddSingleton(typeof(IYodaTranslationService), typeof(YodaTranslationService));
        }
    }
}
