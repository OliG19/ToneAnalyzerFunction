using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ToneAnalyzer.Extensions;
using ToneAnalyzer.Factory;
using ToneAnalyzer.Mappers;
using ToneAnalyzer.Models;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer
{
    public class ToneAnalyzerFunction
    {
        private readonly IToneStrategyFactory _toneStrategyFactory;
        private readonly IToneService _toneService;

        public ToneAnalyzerFunction(
            IToneStrategyFactory toneStrategyFactory, 
            IToneService toneService, 
            ILoggerFactory loggerFactory)
        {
            _toneStrategyFactory = toneStrategyFactory;
            _toneService = toneService;
        }

        [FunctionName("ToneAnalyzerFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tone-analyzer")] HttpRequestMessage request,
            [CosmosDB(
                 "Tones",
                 "Items",
                Id = "id", ConnectionStringSetting = "Cosmos")] IAsyncCollector<FinalTone> output)
        {
            var comment = await request.GetValidComment();

            if (string.IsNullOrWhiteSpace(comment.Text))
            {
                return new BadRequestObjectResult("Please pass a comment as a text property in the request body");
            }

            var tone = await CreateTone(comment);

            await output.AddAsync(tone);

            return new OkObjectResult(tone);
        }

        private async Task<FinalTone> CreateTone(Comment comment)
        {
            var tones = await _toneService.GetTonesAsync(comment);

            var dominantTone = DominantToneFactory.CreateDominantTone(tones);

            var toneStrategy = _toneStrategyFactory.Create(dominantTone.Name);

            var finalTone = FinalToneMapper.MapAsync(comment.Text, dominantTone);

            var tone = await toneStrategy.SetFinalToneJoke(finalTone);

            return tone;
        }
    }
}
;