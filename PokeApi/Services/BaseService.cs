using PokeApi.Infrastracture.Exceptions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeApi.Services
{
    public class BaseService
    {
        protected async Task<T> CheckStatusCodeAndSerializetResultAsync<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize
                    <T>(responseString, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                return result;
            }
            else
            {
                throw new StatusCodeException(response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
