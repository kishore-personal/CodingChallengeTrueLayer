using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using TLPokemon.Application.Exceptions;
using TLPokemon.Application.Interfaces.Apis;
using TLPokemon.Domain.Entities;

namespace TLPokemon.Infrastructure.Persistence.HttpClients
{
    public class PokemonApiClient : IPokemonApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly string _url;
        private readonly HttpClient _httpClient;

        public PokemonApiClient(HttpClient client, IConfiguration configuration)
        {
            _httpClient = client;
            _configuration = configuration;
            _url = _configuration["ApiSettings:PokemonApiUrl"];
        }

        public async Task<Pokemon> GetPokemonByName(string pokemonName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                throw new Exception("Pokemon's name is mandatory");
            }
            string urlSafeText = Uri.EscapeDataString(pokemonName.Replace("\n", " "));
            var requestUrl = new Uri(_url + urlSafeText);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                throw new TooManyRequestsException();
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException(pokemonName + " is not a pokemon");

            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<Pokemon>(contentStream, new JsonSerializerOptions(), cancellationToken);
            }
        }
    }

}
