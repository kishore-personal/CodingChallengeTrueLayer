using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLPokemon.Application.Interfaces.Repositories
{
    public interface IYodaTranslationApiService
    {
        Task<string> TranslateToYoda(string text, CancellationToken cancellationToken);
    }
}
