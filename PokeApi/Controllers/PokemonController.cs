using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokeApi.Models;
using PokeApi.Services.Funtranslations;
using PokeApi.Services.PokeApi;
using System.Threading.Tasks;

namespace PokeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : Controller
    {
        private readonly IPokeApiService pokeApiService;
        private readonly IFuntranslationsService funtranslationsService;

        public PokemonController(IPokeApiService pokeApiService, IFuntranslationsService funtranslationsService)
        {
            this.pokeApiService = pokeApiService;
            this.funtranslationsService = funtranslationsService;
        }

        [HttpGet("{pokemonName}")]
        public async Task<ActionResult<PokemonModel>> GetAsync(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                return BadRequest();
            }

            var model = await pokeApiService.GetPokemonAsync(pokemonName);
            return Json(model);
        }

        [HttpGet("translated/{pokemonName}")]
        public async Task<ActionResult<PokemonModel>> GetTranslatedAsync(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                return BadRequest();
            }

            var model = await pokeApiService.GetPokemonAsync(pokemonName);

            model = await funtranslationsService.TranslatePokemonModelAsync(model);

            return Json(model);
        }
    }
}
