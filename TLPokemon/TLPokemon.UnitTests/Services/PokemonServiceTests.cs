using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces.Repositories;
using TLPokemon.Application.Interfaces.Services;
using TLPokemon.Infrastructure.Shared.Services;
using TLPokemon.Domain.Entities;
using System.Collections.Generic;
using TLPokemon.Application.DTOs.Pokemon;
using TLPokemon.UnitTests.Helpers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TLPokemon.Application.Wrappers;

namespace TLPokemon.UnitTests.Services
{
    [TestFixture]
    public class PokemonServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IPokemonApiService> mockPokemonApiService;
        private Mock<IMemoryCache> mockMemoryCache;
        private Mock<IShakespeareTranslationApiService> mockShakespeareTranslationApiService;
        private Mock<IYodaTranslationApiService> mockYodaTranslationApiService;
        private Mock<IMapper> mockMapper;
        private Mock<ILogger<PokemonService>> mockLogger;
        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockPokemonApiService = this.mockRepository.Create<IPokemonApiService>();
            this.mockMemoryCache = this.mockRepository.Create<IMemoryCache>();
            this.mockShakespeareTranslationApiService = this.mockRepository.Create<IShakespeareTranslationApiService>();
            this.mockYodaTranslationApiService = this.mockRepository.Create<IYodaTranslationApiService>();
            this.mockMapper =  this.mockRepository.Create<IMapper>();
            this.mockLogger = this.mockRepository.Create<ILogger<PokemonService>>();
            mockPokemonApiService.Setup(r => r.GetPokemonData(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new Pokemon { Description = "testD", 
                        Flavors = new List<Flavor> { new Flavor 
                                      { FlavorText = "testF", 
                                        Language =  new Language{ Name = "testL" } 
                                    }
                        } ,
                        IsLegendary =true,
                        Habitat= new Habitat { HabitatName="testH"}
                    });

            var cacheEntryMock = new Mock<ICacheEntry>();
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            mockMapper.Setup(m => m.Map<PokemonResponse>(It.IsAny<Pokemon>()))
                    .Returns(new PokemonResponse());

        }

        private IPokemonService CreateService()
        {
            return new PokemonService(
                this.mockPokemonApiService.Object,
                this.mockMemoryCache.Object,
                this.mockShakespeareTranslationApiService.Object,
                this.mockYodaTranslationApiService.Object,
                this.mockMapper.Object,
                this.mockLogger.Object
                );
        }

        [Test]
        public async Task GivenAPokemon_WhenCallingGetPokemonData_ThenReturnAPokemonResponse()
        {
            // Given
            var service = this.CreateService();
            string name = "mewtwo";
            CancellationToken cancellationToken = default(CancellationToken);

            // When
            var result = await service.GetPokemon(
                name,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<PokemonResponse>(result);
            this.mockPokemonApiService.Verify();
        }
        [Test]
        public void GivenAnEmptyPokemon_WhenCallingGetPokemonData_ThenReturnPokemonNotFound()
        {
            // Given
            var service = this.CreateService();
            string name = String.Empty;
            CancellationToken cancellationToken = default(CancellationToken);

            // When 
            var exception = Assert.ThrowsAsync<Exception>(async () => await service.GetPokemon(
                name,
                cancellationToken));

            // Then
            Assert.That(exception.Message, Is.EqualTo("Pokemon's name is mandatory"));
            this.mockPokemonApiService.Verify();
        }

        [Test]
        public async Task GivenAPokemon_WhenCallingGetTransaltedPokemon_ThenReturnAPokemonResponse()
        {
            // Given
            var service = this.CreateService();
            string name = "mewtwo";
            CancellationToken cancellationToken = default(CancellationToken);

            // When
            var result = await service.GetTransaltedPokemon(
                name,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<PokemonResponse>(result);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<bool>(result.IsLegendary);
            this.mockPokemonApiService.Verify();
        }
    }
}
