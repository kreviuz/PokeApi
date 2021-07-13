using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PokeApi.DTO.PokeApi;
using PokeApi.Mappers;
using PokeApi.Models;
using PokeApi.Services.PokeApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PokeApiTests
{
    [TestFixture]
    public class PokeApiServiceTests
    {
        private IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"PokeApi", "https://pokeapi.co/api/v2"},
                {"FuntranslationsApi", "https://api.funtranslations.com"},
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Test]
        public async Task PokeApiService_ShouldReturnDto()
        {
            //Arrange
            var model = new PokemonSpeciesDto
            {
                Name = "n",
                FlavorTextEntries = new[] { new FlavorTextEntry() { FlavorText = "text", Language = new Language() { Name = "en" } } },
                IsLegendary = true,
                Habitat = new PokemonHabitatDto { Name = "forest" }
            };

            var pokeModel = new PokemonModel();
            
            var mockFactory = SetupHttpFactory(model, HttpStatusCode.OK);

            var mapper = new Mock<IPokemonMapper>();

            mapper.Setup(m => m.MapPokemonSpeciesDtoToPokemonModel(It.IsAny<PokemonSpeciesDto>())).Returns(pokeModel);

            var pokeApiService = new PokeApiService(mockFactory.Object, configuration, mapper.Object);

            //Act
            var result = await pokeApiService.GetPokemonAsync("n");

            //Assert
            Assert.AreEqual(result, pokeModel);

        }

        [Test]
        public async Task FuntranslationsService_ShouldThrowArgumantNullException()
        {
            //Arrange
            var model = new PokemonSpeciesDto
            {
                Name = "n",
                FlavorTextEntries = new[] { new FlavorTextEntry() { FlavorText = "text", Language = new Language() { Name = "en" } } },
                IsLegendary = true,
                Habitat = new PokemonHabitatDto { Name = "forest" }
            };

            var pokeModel = new PokemonModel();

            var mockFactory = SetupHttpFactory(model, HttpStatusCode.OK);

            var mapper = new Mock<IPokemonMapper>();

            mapper.Setup(m => m.MapPokemonSpeciesDtoToPokemonModel(It.IsAny<PokemonSpeciesDto>())).Returns(pokeModel);

            var pokeApiService = new PokeApiService(mockFactory.Object, configuration, mapper.Object);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await pokeApiService.GetPokemonAsync(null));

        }

        private Mock<IHttpClientFactory> SetupHttpFactory(PokemonSpeciesDto pokemonSpeciesDto, HttpStatusCode status = HttpStatusCode.OK)
        {
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent(JsonSerializer.Serialize(pokemonSpeciesDto))
                });



            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            return mockFactory;
        }
    }
}