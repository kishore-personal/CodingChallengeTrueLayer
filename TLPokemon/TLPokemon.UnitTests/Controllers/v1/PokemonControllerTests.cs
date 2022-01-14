using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TLPokemon.Application.DTOs.Pokemon;
using TLPokemon.Application.Interfaces.Services;
using TLPokemon.Application.Wrappers;
using TLPokemon.UnitTests.Helpers;
using TLPokemon.WebApi.Controllers;

namespace TLPokemon.UnitTests.Controllers.v1
{
    [TestFixture]
    public class PokemonControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IPokemonService> mockPokemonService;
        private Mock<ILogger<PokemonController>> mockLogger;
        private Mock<IShakespeareTranslationService> mockTranslationService;
        private int logCounter=2;
        [SetUp]
        public void SetUp()
        {
            
            mockRepository = new MockRepository(MockBehavior.Strict);
            mockPokemonService = this.mockRepository.Create<IPokemonService>();
            mockLogger = new Mock<ILogger<PokemonController>>();
            mockTranslationService = this.mockRepository.Create<IShakespeareTranslationService>();
            mockPokemonService.Setup(r => r.GetPokemon(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new PokemonResponse { Description = "testD", Habitat = "testH", IsLegendary = true, Name = "mewtwo" });
            mockPokemonService.Setup(r => r.GetTransaltedPokemon(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(
               new PokemonResponse { Description = "testD", Habitat = "testH", IsLegendary = true, Name = "mewtwo" });

            mockLogger.MockLog(LogLevel.Information);
        }

        private PokemonController CreatePokemonController()
        {
            return new PokemonController(
                this.mockPokemonService.Object,
                this.mockLogger.Object
                );
        }

        [Test]
        public async Task GivenAPokemonName_WhenCallingGetPokemon_ThenReturnPokemonResponse()
        {
            // Given
            var pokemonController = this.CreatePokemonController();
            string pokemonName = "mewtwo";
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // When
            var result = await pokemonController.GetPokemon(
                pokemonName,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<PokemonResponse>(result.Value);
            Assert.IsInstanceOf<bool>(result.Value.IsLegendary);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(logCounter, mockLogger.Invocations.Count);
            Assert.IsNotNull(result.Value.Name);
            Assert.IsNotNull(result.Value.Description);
            Assert.IsNotNull(result.Value.Habitat);
            Assert.IsNotNull(result.Value.IsLegendary);
            this.mockPokemonService.Verify();
        }
        [Test]
        public async Task GivenAPokemonName_WhenCallingGetTranslatedPokemon_ThenReturnTranslatedPokemonResponse()
        {
            // Given
            var pokemonController = this.CreatePokemonController();
            string pokemonName = "zubat";
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // When
            var result = await pokemonController.GetTransaltedPokemon(
                pokemonName,
                cancellationToken);

            // Then
            Assert.IsInstanceOf<PokemonResponse>(result.Value);
            Assert.AreEqual(logCounter, mockLogger.Invocations.Count);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<bool>(result.Value.IsLegendary);
            Assert.IsNotNull(result.Value.Name);
            Assert.IsNotNull(result.Value.Description);
            Assert.IsNotNull(result.Value.Habitat);
            Assert.IsNotNull(result.Value.IsLegendary);
            this.mockTranslationService.Verify();
            
        }
       
    }
}
