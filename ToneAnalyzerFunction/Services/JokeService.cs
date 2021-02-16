using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Models.Configuration;

namespace ToneAnalyzerFunction.Services
{
    public class JokeService : IJokeService
    {
        public async Task<IEnumerable<Joke>> Get()
        {
            try
            {
                var configuration = GetJokeConfiguration();

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{configuration.JokeUrl}"),
                    Headers =
                    {
                        {"x-rapidapi-key", $"{configuration.JokeApikey}"},
                        {"x-rapidapi-host", $"{configuration.JokeApiHost}"}
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var jokeResponse = JsonConvert.DeserializeObject<JokeResponse>(responseContent);

                    return jokeResponse.Body;
                }
            }
            catch (Exception exception)
            {
                throw new ArgumentException(exception.Message);
            }
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
