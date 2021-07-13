using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PokeApi.Controllers;
using PokeApi.Models;
using PokeApi.Services.Funtranslations;
using PokeApi.Services.PokeApi;
using System.Threading.Tasks;

namespace PokeApiTests
{
    [TestFixture]
    public class PokemonControllerTests
    {
        private PokemonController controller;
        private Mock<IPokeApiService> mockPokeService;
        private Mock<IFuntranslationsService> mockFuntranslationsService;

        [SetUp]
        public void Setup()
        {
            mockFuntranslationsService = new Mock<IFuntranslationsService>();
            mockPokeService = new Mock<IPokeApiService>();

            controller = new PokemonController(mockPokeService.Object, mockFuntranslationsService.Object);
        }

        [Test]
        public async Task GetAsync_ShouldReturnOk()
        {
            //Arrange
            var pokeName = "n";

            var pokeModel = new PokemonModel { Name = "n" };

            mockPokeService.Setup(x => x.GetPokemonAsync(pokeName)).ReturnsAsync(pokeModel);

            //Act
            var actionResult = await controller.GetAsync(pokeName);

            //Assert
            var jsonResult = actionResult.Result as JsonResult;
            Assert.AreEqual(pokeModel, jsonResult.Value);
        }

        [Test]
        public async Task GetAsync_ShouldReturnBadRequest()
        {
            //Act
            var actionResult = await controller.GetAsync(null);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(actionResult.Result);
        }

        [Test]
        public async Task GetTranslatedAsync_ShouldReturnOk()
        {
            //Arrange
            var pokeName = "n";

            var pokeModel = new PokemonModel { Name = "n" };

            mockPokeService.Setup(x => x.GetPokemonAsync(pokeName)).ReturnsAsync(pokeModel);

            mockFuntranslationsService.Setup(x => x.TranslatePokemonModelAsync(pokeModel)).ReturnsAsync(pokeModel);

            //Act
            var actionResult = await controller.GetTranslatedAsync(pokeName);

            //Assert
            var jsonResult = actionResult.Result as JsonResult;
            Assert.AreEqual(pokeModel, jsonResult.Value);
        }

        [Test]
        public async Task GetTranslatedAsync_ShouldReturnBadRequest()
        {
            //Act
            var actionResult = await controller.GetTranslatedAsync(null);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(actionResult.Result);
        }
    }
}