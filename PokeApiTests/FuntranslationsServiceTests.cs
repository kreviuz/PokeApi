using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PokeApi.Models;
using PokeApi.Services.Funtranslations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PokeApiTests
{
    [TestFixture]
    public class FuntranslationsServiceTests
    {
        private IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'success':{'total': 1}, 'contents': {'translated': 'translatedText'}}"),
                });



            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var inMemorySettings = new Dictionary<string, string> {
                {"PokeApi", "https://pokeapi.co/api/v2"},
                {"FuntranslationsApi", "https://api.funtranslations.com"},
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Test]
        public async Task FuntranslationsService_ShouldReturnChangedModel()
        {
            //Arrange
            var model = new PokemonModel
            {
                Description = "d",
                Habitat = "h",
                Name =  "n"
            };

            var mockFactory = SetupHttpFactory("translatedText", 1);

            var funtranslationsService = new FuntranslationsService(mockFactory.Object, configuration);

            //Act
            var result = await funtranslationsService.TranslatePokemonModelAsync(model);

            //Assert
            Assert.AreEqual(result.Description, "translatedText");
            Assert.AreEqual(result.Habitat, "translatedText");

        }

        [Test]
        public async Task FuntranslationsService_ShouldReturnUnchangedModel_IfNotSuccessfulTranslate()
        {
            //Arrange
            var model = new PokemonModel
            {
                Description = "d",
                Habitat = "h",
                Name = "n"
            };

            var mockFactory = SetupHttpFactory("translatedText", 0);

            var funtranslationsService = new FuntranslationsService(mockFactory.Object, configuration);

            //Act
            var result = await funtranslationsService.TranslatePokemonModelAsync(model);

            //Assert
            Assert.AreEqual(result.Description, "d");
            Assert.AreEqual(result.Habitat, "h");

        }

        [Test]
        public async Task FuntranslationsService_ShouldReturnUnchangedModel_IfRequestError()
        {
            //Arrange
            var model = new PokemonModel
            {
                Description = "d",
                Habitat = "h",
                Name = "n"
            };

            var mockFactory = SetupHttpFactory("translatedText", 0, HttpStatusCode.InternalServerError);

            var funtranslationsService = new FuntranslationsService(mockFactory.Object, configuration);

            //Act
            var result = await funtranslationsService.TranslatePokemonModelAsync(model);

            //Assert
            Assert.AreEqual(result.Description, "d");
            Assert.AreEqual(result.Habitat, "h");

        }

        [Test]
        public async Task FuntranslationsService_ShouldThrowArgumantNullException()
        {
            //Arrange
            var model = new PokemonModel
            {
                Description = "d",
                Habitat = "h",
                Name = "n"
            };

            var mockFactory = SetupHttpFactory("translatedText", 0, HttpStatusCode.InternalServerError);

            var funtranslationsService = new FuntranslationsService(mockFactory.Object, configuration);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await funtranslationsService.TranslatePokemonModelAsync(null));

        }

        private Mock<IHttpClientFactory> SetupHttpFactory(string text, int successTotal, HttpStatusCode status = HttpStatusCode.OK)
        {
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent($"{{\"success\":{{\"total\": {successTotal}}}, \"contents\": {{\"translated\": \"{text}\"}}}}"),
                });



            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            return mockFactory;
        }
    }
}