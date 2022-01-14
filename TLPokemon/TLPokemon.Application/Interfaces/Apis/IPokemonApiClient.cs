using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Domain.Entities;

namespace TLPokemon.Application.Interfaces.Apis
{
    public interface IPokemonApiClient
    {
        public Task<Pokemon> GetPokemonByName(string pokemonName, CancellationToken cancellationToken);
    }
}
