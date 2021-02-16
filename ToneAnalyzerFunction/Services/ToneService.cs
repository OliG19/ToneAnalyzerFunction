using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Models.Configuration;

namespace ToneAnalyzerFunction.Services
{
    public class ToneService : IToneService
    {
        private readonly WatsonConfiguration _watsonConfiguration;

        public ToneService(WatsonConfiguration watsonConfiguration)
        {
            _watsonConfiguration = watsonConfiguration;
        }

        public async Task<IEnumerable<Tone>> GetTones(Comment comment)
        {
            var responseContent = await GetToneAsync(comment);

            var formattedResponse = FormatResponse(responseContent);

            var toneResponse = JsonConvert.DeserializeObject<ToneResponse>(formattedResponse);

            return toneResponse.DocumentTone.Tones;
        }

        private async Task<string> GetToneAsync(Comment comment)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes("apikey:" + $"{_watsonConfiguration.WatsonApiKey}")));

            var postData = "{\"text\": \"" + $"{comment.Text}" + "\"}";
            var content = new StringContent(postData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_watsonConfiguration.WatsonUrl}", content);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            return responseContent;
        }

        private string FormatResponse(string responseContent)
        {
            //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

            //responseContent.Keys.Select(k => k.Replace("_", ""));

            var formattedResponse = responseContent.Replace("_", "");

            //var json = JsonConvert.SerializeObject(values);

            //return json;

            return formattedResponse;
        }
    }
}
