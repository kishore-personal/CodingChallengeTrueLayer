using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces.Repositories;
using TLPokemon.Domain.Entities;
using TLPokemon.Infrastructure.Persistence.HttpClients;
using TLPokemon.Infrastructure.Shared.Services;

namespace TLPokemon.UnitTests.Services
{
    [TestFixture]
    public class ShakespeareTranslationServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IShakespeareTranslationApiService> mockShakespeareTranslationApiService;
        private Mock<IMemoryCache> mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockShakespeareTranslationApiService = this.mockRepository.Create<IShakespeareTranslationApiService>();
            this.mockMemoryCache = this.mockRepository.Create<IMemoryCache>();

            mockShakespeareTranslationApiService.Setup(r=>r.TranslateToShakespeare(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("translated");

            var cacheEntryMock = new Mock<ICacheEntry>();
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);
        }

        private ShakespeareTranslationService CreateService()
        {
            return new ShakespeareTranslationService(
                this.mockShakespeareTranslationApiService.Object,
                this.mockMemoryCache.Object);
        }

        [Test]
        public async Task GivenAText_whenCallingShakespeareTranslation_ReturnAShakespeareTranslatedString()
        {
            // Given
            var service = this.CreateService();
            string text = "description to be translated";
            CancellationToken cancellationToken = default(CancellationToken);

            // When
            var result = await service.ShakespeareTranslation(
                text,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<string>(result);
          //  this.mockShakespeareApiClient.Verify();
        }
    }
}
