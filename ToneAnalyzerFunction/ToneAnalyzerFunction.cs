using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ToneAnalyzer.Extensions;
using ToneAnalyzer.Mappers;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Services;

namespace ToneAnalyzer
{
    public class ToneAnalyzerFunction
    {
        private readonly IDominantToneMapper _dominantToneMapper;
        private readonly IToneService _toneService;
        private readonly IJokeService _jokeService;
        private readonly ILogger _logger;

        public ToneAnalyzerFunction(IDominantToneMapper dominantToneMapper, IToneService toneService, ILoggerFactory loggerFactory, IJokeService jokeService)
        {
            _dominantToneMapper = dominantToneMapper;
            _toneService = toneService;
            _jokeService = jokeService;
            _logger = loggerFactory.CreateLogger<ToneAnalyzerFunction>();
        }

        [FunctionName("ToneAnalyzerFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tone-analyzer")] HttpRequestMessage request,
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

            var tones = await _toneService.GetTonesAsync(comment);

            var dominantTone = _dominantToneMapper.Create(tones);

            var mapper = dominantTone.DominantToneMapper(_jokeService);

            var toneToSave = await mapper.MapAsync(comment.Text, dominantTone);

            await output.AddAsync(toneToSave);

            return new OkObjectResult(toneToSave);
        }
    }
}
;