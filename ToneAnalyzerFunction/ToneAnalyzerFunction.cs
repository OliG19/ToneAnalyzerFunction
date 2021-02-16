using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Mappers;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Models.Configuration;
using ToneAnalyzerFunction.Services;

namespace ToneAnalyzerFunction
{
    public class ToneAnalyzerFunction
    {
        private readonly IDominantToneMapper _toneMapperMapper;
        public ToneAnalyzerFunction(IDominantToneMapper toneMapperMapper)
        {
            _toneMapperMapper = toneMapperMapper;
        }

        [FunctionName("ToneAnalyzerFunction")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tone-analyzer")] HttpRequestMessage request,
            [CosmosDB(
                 "Tones",
                 "Items",
                Id = "id",
                ConnectionStringSetting = "CosmosDBConnectionString")] IAsyncCollector<FinalTone> output)
        {
            var commentRequest = await CreateCommentRequest(request);

            var toneService = new ToneService(new WatsonConfiguration());

            var tones = await toneService.GetTones(commentRequest);

            var dominantTone = _toneMapperMapper.Create(tones);

            var mapper = dominantTone.DominantToneMapper;

            var toneToSave = await mapper.MapAsync(commentRequest.Text, dominantTone);

            await output.AddAsync(toneToSave);

            return request.CreateResponse(HttpStatusCode.OK, toneToSave);
        }

        private static async Task<Comment> CreateCommentRequest(HttpRequestMessage req)
        {
            var request = await req.Content.ReadAsStringAsync();

            var comment = JsonConvert.DeserializeObject<Comment>(request);

            return comment;
        }
    }
}
;