using System.Text.Json.Serialization;

namespace PokeApi.DTO.PokeApi
{
    public class FlavorTextEntry
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; }

        public Language Language { get; set; }
    }
}
