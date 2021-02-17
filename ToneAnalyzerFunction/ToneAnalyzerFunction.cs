using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ToneAnalyzerFunction.Extensions;
using ToneAnalyzerFunction.Mappers;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Models.Configuration;
using ToneAnalyzerFunction.Services;

namespace ToneAnalyzerFunction
{
    public class ToneAnalyzerFunction
    {
        private readonly IDominantToneMapper _dominantToneMapper;
        private readonly IToneService _toneService;
        private readonly ILogger _logger;

        public ToneAnalyzerFunction(IDominantToneMapper dominantToneMapper, IToneService toneService, ILoggerFactory loggerFactory)
        {
            _dominantToneMapper = dominantToneMapper;
            _toneService = toneService;
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

            if (comment.Text == null)
            {
                return new BadRequestObjectResult("Please pass a comment as a text property in the request body");
            }

            var tones = await _toneService.GetTonesAsync(comment);

            var dominantTone = _dominantToneMapper.Create(tones);

            var mapper = dominantTone.DominantToneMapper;

            var toneToSave = await mapper.MapAsync(comment.Text, dominantTone);

            await output.AddAsync(toneToSave);

            return new OkObjectResult(toneToSave);
        }
    }
}
;