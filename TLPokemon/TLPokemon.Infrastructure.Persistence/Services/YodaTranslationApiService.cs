using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces.Apis;
using TLPokemon.Application.Interfaces.Repositories;
using TLPokemon.Infrastructure.Persistence.HttpClients;

namespace TLPokemon.Infrastructure.Persistence.Services
{
    public class YodaTranslationApiService : IYodaTranslationApiService
    {
        private readonly IYodaApiClient _apiClient;
        private readonly IMemoryCache _memoryCache;

        public YodaTranslationApiService(IYodaApiClient apiClient, IMemoryCache memoryCache)
        {
            _apiClient = apiClient;
            _memoryCache = memoryCache;
        }

        public async Task<string> TranslateToYoda(string text, CancellationToken cancellationToken)
        {
            if (_memoryCache.TryGetValue(text, out string translated))
            {
                return translated;
            }
            var apiResponse = await _apiClient.TranslateToYoda(text, cancellationToken);
            translated = apiResponse.Contents.Translated;
            _memoryCache.Set(text, translated, TimeSpan.FromSeconds(60));
            return translated;
        }
    }
}
