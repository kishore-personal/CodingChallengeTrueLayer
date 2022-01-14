using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.DTOs.Pokemon;
using TLPokemon.Application.Interfaces.Services;
using TLPokemon.Application.Interfaces.Repositories;
using TLPokemon.Domain.Entities;
using TLPokemon.Domain.Constants;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace TLPokemon.Infrastructure.Shared.Services
{
   public class PokemonService : IPokemonService
    {
        private readonly IPokemonApiService _pokemonApiService;
        private readonly IShakespeareTranslationApiService _shakespeareTranslationApiService;
        private readonly IYodaTranslationApiService _yodaTranslationApiService;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly ILogger<PokemonService> _logger;
        public PokemonService(IPokemonApiService pokemonApiService, 
            IMemoryCache memoryCache, 
            IShakespeareTranslationApiService shakespeareTranslationApiService, 
            IYodaTranslationApiService yodaTranslationApiService, 
            IMapper mapper,
             ILogger<PokemonService> logger
            )
        {
            _pokemonApiService = pokemonApiService;
            _memoryCache = memoryCache;
            _shakespeareTranslationApiService = shakespeareTranslationApiService;
            _yodaTranslationApiService = yodaTranslationApiService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PokemonResponse> GetPokemon(string name, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Begin: Check if the pokemon is empty");
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Pokemon's name is mandatory");
            }
            _logger.LogInformation("End: Pokemon is not empty");
            _logger.LogInformation("Begin: Source Pokemon from the external service");
            var pokemon = await _pokemonApiService.GetPokemonData(name, cancellationToken);
            _logger.LogInformation("End: Recieved Pokemon from the external service");
            var pokemonResponse = _mapper.Map<PokemonResponse>(pokemon);
            return pokemonResponse;
        }
        public async Task<PokemonResponse> GetTransaltedPokemon(string name, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Begin: Check if the pokemon is empty");
            if (string.IsNullOrEmpty(name))
                throw new Exception("Pokemon's name is mandatory");
            _logger.LogInformation("End: Pokemon is not empty");
            _logger.LogInformation("Begin: Check for pokemon translation in memory");
            var pokemon = await _pokemonApiService.GetPokemonData(name, cancellationToken);
            var habitat = pokemon.Habitat.HabitatName;
            string translated;
            try
            {
                if (habitat == PokemonConstants.HabitatCave || pokemon.IsLegendary)
                    translated = await _yodaTranslationApiService.TranslateToYoda(pokemon.Description, cancellationToken);
                else
                    translated = await _shakespeareTranslationApiService.TranslateToShakespeare(pokemon.Description, cancellationToken);
                pokemon.Description = translated;
            }
            catch
            {
                translated = pokemon.Description;
            }
            var pokemonResponse = _mapper.Map<PokemonResponse>(pokemon);
            return pokemonResponse;
        }
    }
}
