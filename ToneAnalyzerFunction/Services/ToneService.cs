using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Models.Configuration;

namespace ToneAnalyzer.Services
{
    public class ToneService : IToneService
    {
        private static HttpClient Client = new HttpClient();

        public async Task<IEnumerable<Tone>> GetTonesAsync(Comment comment)
        {
            try
            {
                var responseContent = await GetToneResponseAsync(comment);

                var formattedResponse = FormatResponse(responseContent);

                var toneResponse = JsonConvert.DeserializeObject<ToneResponse>(formattedResponse);

                return toneResponse.DocumentTone.Tones;
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }

        private async Task<string> GetToneResponseAsync(Comment comment)
        {
            SetAuthorization();

            var content = CreateContent(comment);

            var response = await Client.PostAsync($"{WatsonConfiguration.WatsonUrl}", content);

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return responseContent;
        }

        private static StringContent CreateContent(Comment comment)
        {
            var postData = "{\"text\": \"" + $"{comment.Text}" + "\"}";
            var content = new StringContent(postData, Encoding.UTF8, "application/json");
            return content;
        }

        private static void SetAuthorization()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes("apikey:" + $"{WatsonConfiguration.WatsonApiKey}")));
        }

        private string FormatResponse(string responseContent)
            => responseContent.Replace("_", "");
    }
}
