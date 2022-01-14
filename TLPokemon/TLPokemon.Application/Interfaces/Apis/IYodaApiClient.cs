using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Domain.Entities;

namespace TLPokemon.Application.Interfaces.Apis
{
	public interface IYodaApiClient
	{
		public Task<ShakespeareApiResponse> TranslateToYoda(string description, CancellationToken cancellationToken);
	}
}
