using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLPokemon.Application.Interfaces.Services
{
    public interface IShakespeareTranslationService
    {
        Task<string> ShakespeareTranslation(string text, CancellationToken cancellationToken);
    }
}
