using Flurl;
using Microsoft.Extensions.Configuration;
using PokeApi.DTO.Funtranslations;
using PokeApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokeApi.Services.Funtranslations
{
    public class FuntranslationsService : BaseService, IFuntranslationsService
    {
        private readonly IHttpClientFactory clientFactory;

        private readonly string funtranslationsApiLink;

        private const string YodaLocation = "yoda.json";
        private const string ShakespeareLocation = "shakespeare.json";


        public FuntranslationsService(IHttpClientFactory clientFactory, IConfiguration config)
        {
            this.clientFactory = clientFactory;
            funtranslationsApiLink = config.GetValue<string>("FuntranslationsApi");
        }

        public async Task<PokemonModel> TranslatePokemonModelAsync(PokemonModel pokemonModel)
        {
            if (pokemonModel == null)
            {
                throw new ArgumentNullException(nameof(pokemonModel));
            }

            var translationLocation = GetTranslationLocation(pokemonModel);

            try
            {

                pokemonModel.Description = await TranslateTextAsync(pokemonModel.Description, translationLocation);
                pokemonModel.Habitat = await TranslateTextAsync(pokemonModel.Habitat, translationLocation);
                return pokemonModel;
            }
            catch (Exception)
            {
                return pokemonModel;
            }
        }

        private async Task<string> TranslateTextAsync(string text, string translationLocation)
        {
            var client = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post,
               Url.Combine(funtranslationsApiLink, "translate", translationLocation));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "text", text },
            });

            var response = await client.SendAsync(request);
            var translationResult = await CheckStatusCodeAndSerializetResultAsync<TranslationResult>(response);

            if (translationResult.Success.Total == 0 || translationResult.Contents == null)
            {
                return text;
            }

            return translationResult.Contents.Translated ?? translationResult.Contents.Text;
        }

        protected virtual string GetTranslationLocation(PokemonModel pokemonModel)
        {
            if (pokemonModel.IsLegendary || pokemonModel.Habitat == "cave")
            {
                return YodaLocation;
            }
            return ShakespeareLocation;
        }
    }
}
