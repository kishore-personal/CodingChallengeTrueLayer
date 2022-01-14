using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces.Apis;
using TLPokemon.Domain.Entities;
using TLPokemon.Infrastructure.Persistence.HttpClients;
using TLPokemon.Infrastructure.Persistence.Services;

namespace TLPokemon.UnitTests.Presistance.Services
{
    [TestFixture]
    public class ShakespeareTranslationApiServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IShakespeareApiClient> mockShakespeareApiClient;
        private Mock<IMemoryCache> mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockShakespeareApiClient = this.mockRepository.Create<IShakespeareApiClient>();
            this.mockMemoryCache = this.mockRepository.Create<IMemoryCache>();
            var cacheEntryMock = new Mock<ICacheEntry>();
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            mockShakespeareApiClient.Setup(x => x.TranslateToShakespeare(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new ShakespeareApiResponse{
                        Contents = new Contents 
                        { 
                            Translated="translated Text"
                        }
                    });

        }

        private ShakespeareTranslationApiService CreateService()
        {
            return new ShakespeareTranslationApiService(
                this.mockShakespeareApiClient.Object,
                this.mockMemoryCache.Object);
        }

        [Test]
        public async Task GivenAPokemon_WhenCallingTranslatToShakespeare_ThenReturnAShakespearePokemonDescriptionResponse()
        {
            // Given 
            var service = this.CreateService();
            string pokemonName = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // When
            var result = await service.TranslateToShakespeare(
                pokemonName,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<string>(result);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            this.mockShakespeareApiClient.Verify();
        }
    }
}
