using PokeApi.Models;
using System.Threading.Tasks;

namespace PokeApi.Services.Funtranslations
{
    public interface IFuntranslationsService
    {
        Task<PokemonModel> TranslatePokemonModelAsync(PokemonModel pokemonModel);
    }
}
