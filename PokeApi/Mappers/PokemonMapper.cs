using PokeApi.DTO.PokeApi;
using PokeApi.Models;
using System;
using System.Linq;

namespace PokeApi.Mappers
{
    public class PokemonMapper : IPokemonMapper
    {
        public PokemonModel MapPokemonSpeciesDtoToPokemonModel(PokemonSpeciesDto pokemonSpecies)
        {
            if (pokemonSpecies == null)
            {
                throw new ArgumentNullException(nameof(pokemonSpecies));
            }

            var pokemonModel = new PokemonModel();
            pokemonModel.IsLegendary = pokemonSpecies.IsLegendary;
            pokemonModel.Habitat = pokemonSpecies.Habitat?.Name;
            pokemonModel.Name = pokemonSpecies.Name;
            pokemonModel.Description = pokemonSpecies.FlavorTextEntries.FirstOrDefault(x => x.Language?.Name == "en")?.FlavorText;

            return pokemonModel;
        }
    }
}
