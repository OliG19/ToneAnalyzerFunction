using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Models.Configuration;

namespace ToneAnalyzerFunction.Services
{
    public class JokeService : IJokeService
    {
        private static HttpClient Client = new HttpClient();

        public async Task<IEnumerable<Joke>> Get()
        {
            try
            {
                var configuration = GetJokeConfiguration();

                CreateHeaders(configuration);

                var response = await Client.GetAsync($"{configuration.JokeUrl}");

                return await CreateJokeList(response);
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }

        private static async Task<IEnumerable<Joke>> CreateJokeList(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            var jokeResponse = JsonConvert.DeserializeObject<JokeResponse>(responseContent);

            return jokeResponse.Body;
        }

        private static void CreateHeaders(JokeConfiguration configuration)
        {
            Client.DefaultRequestHeaders.Add("x-rapidapi-key", $"{configuration.JokeApikey}");
            Client.DefaultRequestHeaders.Add("x-rapidapi-host", $"{configuration.JokeApiHost}");
        }

        private static JokeConfiguration GetJokeConfiguration()
        {
            return new JokeConfiguration
            {
                JokeUrl = Environment.GetEnvironmentVariable("JokeUrl"),
                JokeApikey = Environment.GetEnvironmentVariable("JokeApikey"),
                JokeApiHost = Environment.GetEnvironmentVariable("JokeApiHost"),
            };
        }
    }
}
