using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToneAnalyzer.Models;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static async Task<Comment> GetValidComment(this HttpRequestMessage req)
        {
            try
            {
                var request = await req.Content.ReadAsStringAsync();

                var comment = JsonConvert.DeserializeObject<Comment>(request);

                return comment;
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }
    }
}
