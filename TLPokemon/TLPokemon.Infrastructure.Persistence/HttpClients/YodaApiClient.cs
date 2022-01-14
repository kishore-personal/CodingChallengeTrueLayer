using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Exceptions;
using TLPokemon.Application.Interfaces.Apis;
using TLPokemon.Domain.Entities;

namespace TLPokemon.Infrastructure.Persistence.HttpClients
{
	public class YodaApiClient : IYodaApiClient
	{
		private readonly IConfiguration _configuration;
		private readonly string _url;
		private readonly HttpClient _httpClient;

		public YodaApiClient(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_url = _configuration["ApiSettings:YodaApiUrl"];
		}

		public async Task<ShakespeareApiResponse> TranslateToYoda(string description, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(description))
			{
				return null;
			}
			string urlSafeText = Uri.EscapeDataString(description.Replace("\n", " "));
			var requestUrl = new Uri(_url + urlSafeText);
			var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
			var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

			if (response.StatusCode == HttpStatusCode.TooManyRequests)
				throw new TooManyRequestsException();
			
			using (var contentStream = await response.Content.ReadAsStreamAsync())
			{
				return await JsonSerializer.DeserializeAsync<ShakespeareApiResponse>(contentStream, new JsonSerializerOptions(), cancellationToken);
			}
		}
	}

}
