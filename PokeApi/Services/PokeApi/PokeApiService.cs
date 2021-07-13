using Flurl;
using Microsoft.Extensions.Configuration;
using PokeApi.DTO.PokeApi;
using PokeApi.Mappers;
using PokeApi.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokeApi.Services.PokeApi
{
    public class PokeApiService : BaseService, IPokeApiService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IPokemonMapper pokemonMapper;

        private readonly string pokeApiLink;

        private const string pokemonSpeciesPath = "pokemon-species";

        public PokeApiService(IHttpClientFactory clientFactory, IConfiguration config, IPokemonMapper pokemonMapper)
        {
            this.clientFactory = clientFactory;
            this.pokemonMapper = pokemonMapper;
            pokeApiLink = config.GetValue<string>("PokeApi");
        }

        public async Task<PokemonModel> GetPokemonAsync(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                throw new ArgumentNullException(nameof(pokemonName));
            }

            var client = clientFactory.CreateClient();

            var pokemonSpecies = await GetPokemonSpecies(client, pokemonName);

            var pokemonModel = pokemonMapper.MapPokemonSpeciesDtoToPokemonModel(pokemonSpecies);

            return pokemonModel;
        }

        private async Task<PokemonSpeciesDto> GetPokemonSpecies(HttpClient client, string pokemonName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                Url.Combine(pokeApiLink, pokemonSpeciesPath, pokemonName));

            var response = await client.SendAsync(request);

            return await CheckStatusCodeAndSerializetResultAsync<PokemonSpeciesDto>(response);
        }
    }
}
