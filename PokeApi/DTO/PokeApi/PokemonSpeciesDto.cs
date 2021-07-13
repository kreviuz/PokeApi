using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokeApi.DTO.PokeApi
{
    public class PokemonSpeciesDto
    {
        public string Name { get; set; }
        public PokemonHabitatDto Habitat { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public IEnumerable<FlavorTextEntry> FlavorTextEntries { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }
}
