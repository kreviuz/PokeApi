using PokeApi.Models;
using System.Threading.Tasks;

namespace PokeApi.Services.PokeApi
{
    public interface IPokeApiService
    {
        Task<PokemonModel> GetPokemonAsync(string pokemonName);
    }
}
