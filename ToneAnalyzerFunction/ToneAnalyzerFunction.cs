using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ToneAnalyzer.Extensions;
using ToneAnalyzer.Mappers;
using ToneAnalyzer.Models;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer
{
    public class ToneAnalyzerFunction
    {
        private readonly IDominantToneStrategy _dominantToneStrategy;
        private readonly IToneService _toneService;
        private readonly IJokeService _jokeService;

        public ToneAnalyzerFunction(IDominantToneStrategy dominantToneStrategy, IToneService toneService, ILoggerFactory loggerFactory, IJokeService jokeService)
        {
            _dominantToneStrategy = dominantToneStrategy;
            _toneService = toneService;
            _jokeService = jokeService;
        }

        [FunctionName("ToneAnalyzerFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tone-analyzer")] HttpRequestMessage request,
            [CosmosDB(
                 "Tones",
                 "Items",
                Id = "id",
                ConnectionStringSetting = "CosmosDBConnectionString")] IAsyncCollector<FinalTone> output)
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

            var dominantTone = _dominantToneStrategy.Create(tones);

            var mapper = dominantTone.DominantToneMapper(_jokeService);

            var tone = await mapper.MapAsync(comment.Text, dominantTone);

            return tone;
        }
    }
}
;