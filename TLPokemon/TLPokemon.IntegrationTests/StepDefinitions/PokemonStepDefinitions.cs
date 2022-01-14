using RestSharp;
using System;
using TechTalk.SpecFlow;
using NUnit;
using NUnit.Framework;
using Newtonsoft.Json;
using TLPokemon.Application.DTOs.Pokemon;
using System.Text.Json;
using System.Threading;
using TLPokemon.Domain.Entities;
using TLPokemon.IntegrationTests.HelperFactories;
using System.Net;
using TLPokemon.Application.Wrappers;
using Newtonsoft.Json.Linq;

namespace TLPokemon.IntegrationTests.StepDefinitions
{
    [Binding]
    public class PokemonStepDefinitions : ApiWebApplicationFactory
    {
        private ScenarioContext _scenarioContext;
        private HttpClient httpClient;
        private ApiWebApplicationFactory webApplicationFactory;
        public PokemonStepDefinitions(ScenarioContext scenarioContext, ApiWebApplicationFactory webApplicationFactory)
        {
            _scenarioContext = scenarioContext;
            this.webApplicationFactory = webApplicationFactory;
            this.httpClient = webApplicationFactory.CreateClient();
        }
        [Given(@"the pokemon name is '(.*)'")]
        public void GivenThePokemonNameIs(string name)
        {
            _scenarioContext["name"] = name;
        }
       
        [When(@"calling the endpoint GetPokemonData")]
        public async Task  WhenCallingTheEndpointGetPokemonData()
        {
            var response = await httpClient.GetAsync($"v1/Pokemon/"+ _scenarioContext["name"].ToString());
            _scenarioContext["response"] = response;
            _scenarioContext["responseCode"] = response.StatusCode;
            _scenarioContext["responseIsSuccess"] = response.IsSuccessStatusCode;


          
        }
        [Then(@"I should recieve a pokemon response with description as '([^']*)' habitat as '([^']*)' isLegendry as '([^']*)'")]
        public void ThenIShouldRecieveAPokemonResponseWithDescriptionAsHabitatAsIsLegendryAs(string description, string habitat, string isLegendary)
        {
            var response = (HttpResponseMessage)_scenarioContext["response"];
            var pokemon = JsonConvert.DeserializeObject<PokemonResponse>(response.Content.ReadAsStringAsync().Result);
            _scenarioContext["pokemon"] = pokemon;
            

            Assert.IsInstanceOf<HttpResponseMessage>(_scenarioContext["response"]);
            Assert.AreEqual(HttpStatusCode.OK,response.StatusCode);
            Assert.AreEqual(true,_scenarioContext["responseIsSuccess"]);
            Assert.IsNotNull(pokemon);
            Assert.AreEqual(_scenarioContext["name"], pokemon.Name);
            Assert.AreEqual(description, pokemon.Description);
            Assert.AreEqual(habitat, pokemon.Habitat);
            Assert.AreEqual(Convert.ToBoolean(isLegendary), pokemon.IsLegendary);
        }
        [When(@"calling the endpoint GetTransaltedPokemon")]
        public async Task WhenCallingTheEndpointGetTransaltedPokemon()
        {
            var response = await httpClient.GetAsync($"v1/Pokemon/translated/" + _scenarioContext["name"].ToString());
            _scenarioContext["response"] = response;
            _scenarioContext["responseCode"] = response.StatusCode;
            _scenarioContext["responseIsSuccess"] = response.IsSuccessStatusCode;


            var pokemon = JsonConvert.DeserializeObject<PokemonResponse>(response.Content.ReadAsStringAsync().Result);
            _scenarioContext["pokemon"] = pokemon;
        }

        [Then(@"I should recieve a pokemon response with errorDescription as '([^']*)' errorCode as '([^']*)'")]
        public void ThenIShouldRecieveAPokemonResponseWithErrorDescriptionAsErrorCodeAs(string errorDescription, string errorCode)
        {
            var response = (HttpResponseMessage)_scenarioContext["response"];
            var error = JsonConvert.DeserializeObject<Response<string>>(response.Content.ReadAsStringAsync().Result);

            Assert.IsFalse(error.Succeeded);
            Assert.AreEqual(errorCode,response.StatusCode.ToString());
            
            //Assert.IsInstanceOf<HttpStatusCode>(_scenarioContext["response"]);
            //Assert.AreEqual(HttpStatusCode.OK, _scenarioContext["response"]);
            //Assert.AreEqual(true, _scenarioContext["responseIsSuccess"]);
            //Assert.IsNotNull(pokemon);
            //Assert.AreEqual(_scenarioContext["name"], pokemon.Name);
            //Assert.AreEqual(errorDescription, pokemon.Description);
            //Assert.AreEqual(habitat, pokemon.Habitat);
            //Assert.AreEqual(Convert.ToBoolean(isLegendary), pokemon.IsLegendary);
        }


    }
}
