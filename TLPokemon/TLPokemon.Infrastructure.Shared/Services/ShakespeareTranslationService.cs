using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces.Repositories;
using TLPokemon.Application.Interfaces.Services;
using TLPokemon.Infrastructure.Persistence.HttpClients;

namespace TLPokemon.Infrastructure.Shared.Services
{
    public class ShakespeareTranslationService : IShakespeareTranslationService
    {
        
        private readonly IShakespeareTranslationApiService _shakespeareTranslationApiService;
        private readonly IMemoryCache _memoryCache;

        public ShakespeareTranslationService(IShakespeareTranslationApiService shakespeareTranslationApiService, IMemoryCache memoryCache)
        {
            _shakespeareTranslationApiService = shakespeareTranslationApiService;
            _memoryCache = memoryCache;
        }

        public async Task<string> ShakespeareTranslation(string text, CancellationToken cancellationToken)
        {
            if (_memoryCache.TryGetValue(text, out string translated))
            {
                return translated;
            }

            var apiResponse = await _shakespeareTranslationApiService.TranslateToShakespeare(text, cancellationToken);
            translated = apiResponse;
            _memoryCache.Set(text, translated, TimeSpan.FromSeconds(60));
            return translated;
        }

       
    }
}
