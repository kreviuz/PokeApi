using PokeApi.DTO.PokeApi;
using PokeApi.Models;

namespace PokeApi.Mappers
{
    public interface IPokemonMapper
    {
        public PokemonModel MapPokemonSpeciesDtoToPokemonModel(PokemonSpeciesDto pokemonSpecies);
    }
}
