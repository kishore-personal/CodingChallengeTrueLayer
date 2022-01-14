using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Domain.Entities;
namespace TLPokemon.Application.Interfaces.Apis
{
	public interface IShakespeareApiClient
	{
		public Task<ShakespeareApiResponse> TranslateToShakespeare(string description, CancellationToken cancellationToken);
	}
}
