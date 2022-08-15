using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToneAnalyzer.Models.Configuration;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Services
{
    public class JokeService : IJokeService
    {
        private static HttpClient Client = new HttpClient();

        public async Task<Joke> GetAsync()
        {
            try
            {
                CreateHeaders();

                var response = await Client.GetAsync($"{JokeConfiguration.JokeUrl}");

                return await GetFirstJoke(response);
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }

        private static async Task<Joke> GetFirstJoke(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            var jokeResponse = JsonConvert.DeserializeObject<JokeResponse>(responseContent);

            return jokeResponse.Body.First();
        }

        private static void CreateHeaders()
        {
            Client.DefaultRequestHeaders.Add("x-rapidapi-key", $"{JokeConfiguration.JokeApikey}");
            Client.DefaultRequestHeaders.Add("x-rapidapi-host", $"{JokeConfiguration.JokeApiHost}");
        }
    }
}
