using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.Interfaces.Apis;
using TLPokemon.Domain.Constants;
using TLPokemon.Domain.Entities;
using TLPokemon.Infrastructure.Persistence.HttpClients;
using TLPokemon.Infrastructure.Persistence.Services;

namespace TLPokemon.UnitTests.Presistance.Services
{
    [TestFixture]
    public class PokemonApiServiceTests
    {
        private MockRepository mockRepository;
        private Mock<IPokemonApiClient> mockPokemonApiClient;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IMemoryCache> mockMemoryCache;
        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockPokemonApiClient = this.mockRepository.Create<IPokemonApiClient>();
            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
            this.mockMemoryCache = this.mockRepository.Create<IMemoryCache>();
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.Setup(s => s[It.IsAny<string>()]).Returns("en");

            var cacheEntryMock = new Mock<ICacheEntry>();
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);
            mockPokemonApiClient.Setup(x => x.GetPokemonByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new Pokemon
                    {Description="TestD",
                    Name= "TestN",
                    Habitat= new Habitat { HabitatName="TestH"},
                    
                        Flavors = new List<Flavor> { new Flavor
                                      { FlavorText = "testF",
                                        Language =  new Language{ Name = "en" }
                                    }
                        }
                    });


            var mockIConfigurationSection = new Mock<IConfigurationSection>();
            mockIConfigurationSection.Setup(x => x.Key).Returns(PokemonConstants.LanguageCode);
            mockIConfigurationSection.Setup(x => x.Value).Returns("en");
            mockConfiguration.Setup(x => x.GetSection(PokemonConstants.LanguageCode)).Returns(mockIConfigurationSection.Object);

        }

        private PokemonApiService CreateService()
        {
            return new PokemonApiService(
                this.mockPokemonApiClient.Object,
                this.mockConfiguration.Object,
                this.mockMemoryCache.Object
                );
        }

        [Test]
        public async Task GivenAPokemon_WhenCallingGetPokemonData_ThenReturnAPokemonResponse()
        {
            // Given 
            var service = this.CreateService();
            string pokemonName = "pikachu";
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // When
            var result = await service.GetPokemonData(
                pokemonName,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<Pokemon>(result);
            Assert.IsInstanceOf<bool>(result.IsLegendary);
            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.Description);
            Assert.IsNotNull(result.Habitat);
            Assert.IsNotNull(result.IsLegendary);
            Assert.IsNotEmpty(result.Name);
            Assert.IsNotEmpty(result.Description);
            Assert.IsNotEmpty(result.Habitat.HabitatName);
            
            this.mockPokemonApiClient.Verify();
        }

    }
}
