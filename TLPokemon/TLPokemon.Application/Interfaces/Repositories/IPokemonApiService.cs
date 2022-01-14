using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Domain.Entities;

namespace TLPokemon.Application.Interfaces.Repositories
{
    public interface IPokemonApiService
    {
        Task<Pokemon> GetPokemonData(string pokemonName, CancellationToken cancellationToken);
    }
}
