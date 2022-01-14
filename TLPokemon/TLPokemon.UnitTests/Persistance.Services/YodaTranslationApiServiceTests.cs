using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces.Apis;
using TLPokemon.Domain.Entities;
using TLPokemon.Infrastructure.Persistence.Services;

namespace TLPokemon.UnitTests.Presistance.Services
{
    [TestFixture]
    public class YodaTranslationApiServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IYodaApiClient> mockYodaApiClient;
        private Mock<IMemoryCache> mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockYodaApiClient = this.mockRepository.Create<IYodaApiClient>();
            this.mockMemoryCache = this.mockRepository.Create<IMemoryCache>();
            var cacheEntryMock = new Mock<ICacheEntry>();
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            mockYodaApiClient.Setup(x => x.TranslateToYoda(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new ShakespeareApiResponse
                    {
                        Contents = new Contents
                        {
                            Translated = "translated Text"
                        }
                    });
        }

        private YodaTranslationApiService CreateService()
        {
            return new YodaTranslationApiService(
                this.mockYodaApiClient.Object,
                this.mockMemoryCache.Object);
        }

        [Test]
        public async Task GivenAPokemon_WhenCallingTranslateToYoda_ThenReturnAYodaPokemonDescriptionResponse()
        {
            // Given 
            var service = this.CreateService();
            string pokemonName = "pikachu";
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // When
            var result = await service.TranslateToYoda(
                pokemonName,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<string>(result);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            this.mockYodaApiClient.Verify();
        }
    }
}
