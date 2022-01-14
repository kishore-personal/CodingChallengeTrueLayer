using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.DTOs.Pokemon;
using TLPokemon.Application.Interfaces.Services;

namespace TLPokemon.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class PokemonController : BaseApiController
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger;
       
        public PokemonController(IPokemonService pokemonService, ILogger<PokemonController> logger)                                                              
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        ///  <summary>
        /// To retrieve basic pokemon details from the pokeApi
        ///  </summary>
        [HttpGet]
        [Route("{pokemonName}")]
        public async Task<ActionResult<PokemonResponse>> GetPokemon(string pokemonName, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, "Begin: Pokemon Service");
            var pokemon = await _pokemonService.GetPokemon(pokemonName, cancellationToken);
            _logger.Log(LogLevel.Information, "End: Pokemon Service");
            return pokemon;
        }

        ///  <summary>
        /// To retrieve basic pokemon details from the pokeApi with translated(Shakespear/Yoda) descriptions
        ///  </summary>
        [HttpGet]
        [Route("translated/{pokemonName}")]
        public async Task<ActionResult<PokemonResponse>> GetTransaltedPokemon(string pokemonName, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, "Begin: Pokemon Translation Service");
            var pokemon = await _pokemonService.GetTransaltedPokemon(pokemonName, cancellationToken);
            _logger.Log(LogLevel.Information, "End: Pokemon Translation Service");
            return pokemon;
        }
        
    }
}