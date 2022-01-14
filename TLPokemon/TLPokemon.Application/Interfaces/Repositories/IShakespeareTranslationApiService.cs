using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLPokemon.Application.Interfaces.Repositories
{
    public interface IShakespeareTranslationApiService
    {
        Task<string> TranslateToShakespeare(string text, CancellationToken cancellationToken);
    }
}
