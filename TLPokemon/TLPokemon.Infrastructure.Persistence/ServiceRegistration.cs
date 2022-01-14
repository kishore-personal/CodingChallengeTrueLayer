using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TLPokemon.Application.Interfaces.Repositories;
using TLPokemon.Infrastructure.Persistence.Services;


namespace TLPokemon.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Repositories
            services.AddSingleton(typeof(IPokemonApiService), typeof(PokemonApiService));
            services.AddSingleton(typeof(IShakespeareTranslationApiService), typeof(ShakespeareTranslationApiService));
            services.AddSingleton(typeof(IYodaTranslationApiService), typeof(YodaTranslationApiService));
            #endregion
        }
    }
}
