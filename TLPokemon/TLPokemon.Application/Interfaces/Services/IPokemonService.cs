using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.DTOs.Pokemon;

namespace TLPokemon.Application.Interfaces.Services
{
    public interface IPokemonService
    {
        Task<PokemonResponse> GetPokemon(string name, CancellationToken cancellationToken);
        Task<PokemonResponse> GetTransaltedPokemon(string name, CancellationToken cancellationToken);
        
    }
}
