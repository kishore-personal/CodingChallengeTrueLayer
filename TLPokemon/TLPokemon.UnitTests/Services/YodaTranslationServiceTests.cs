using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces.Repositories;
using TLPokemon.Infrastructure.Persistence.HttpClients;
using TLPokemon.Infrastructure.Shared.Services;

namespace TLPokemon.UnitTests.Services
{
    [TestFixture]
    public class YodaTranslationServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IYodaTranslationApiService> mockYodaTranslationApiService;
        private Mock<IMemoryCache> mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);


            this.mockYodaTranslationApiService = this.mockRepository.Create<IYodaTranslationApiService>();
            this.mockMemoryCache = this.mockRepository.Create<IMemoryCache>();

            mockYodaTranslationApiService.Setup(r => r.TranslateToYoda(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("translated");

            var cacheEntryMock = new Mock<ICacheEntry>();
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);
        }

        private YodaTranslationService CreateService()
        {
            return new YodaTranslationService(
                this.mockYodaTranslationApiService.Object,
                this.mockMemoryCache.Object);
        }

        [Test]
        public async Task GivenAText_whenCallingYodaTranslation_ReturnAYodaTranslatedString()
        {
            // Given
            var service = this.CreateService();
            string text = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // When
            var result = await service.YodaTranslation(
                text,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<string>(result);
            this.mockRepository.VerifyAll();
        }
    }
}
