using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces;
using TLPokemon.Application.Interfaces.Apis;
using TLPokemon.Application.Interfaces.Repositories;
using TLPokemon.Domain.Constants;
using TLPokemon.Domain.Entities;
using TLPokemon.Infrastructure.Persistence.HttpClients;

namespace TLPokemon.Infrastructure.Persistence.Services
{
    public class PokemonApiService : IPokemonApiService
    {
        private readonly IConfiguration _configuration;
        private readonly IPokemonApiClient _pokemonApiClient;
        private readonly string languageCode;
        private readonly IMemoryCache _memoryCache;


        public PokemonApiService(IPokemonApiClient pokemonApiClient, IConfiguration configuration,IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _pokemonApiClient = pokemonApiClient;
            languageCode = _configuration.GetSection(PokemonConstants.LanguageCode).Get<string>();
            _memoryCache = memoryCache;
        }

        public async Task<Pokemon> GetPokemonData(string pokemonName, CancellationToken cancellationToken)
        {
            if (_memoryCache.TryGetValue<Pokemon>(pokemonName, out var pokemon) && pokemon != null)
                return pokemon;
            
            pokemon = await _pokemonApiClient.GetPokemonByName(pokemonName, cancellationToken);
            pokemon.Name = pokemonName;
            pokemon.Description = pokemon.Flavors.FirstOrDefault(fte => fte.Language.Name == languageCode)?.FlavorText;

            _memoryCache.Set(pokemonName, pokemon, TimeSpan.FromSeconds(60));
            return pokemon;
        }
    }
}
