using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;
using ToneAnalyzer.Mappers;
using ToneAnalyzer.Models;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;
using Xunit;

namespace ToneAnalyzerTests
{
    public class ToneAnalyzerFunctionTests
    {
        private readonly Mock<IDominantToneStrategy> _dominantToneMapper;
        private readonly Mock<IToneService> _toneService;
        private readonly Mock<IJokeService> _jokeService;
        private readonly Mock<ILoggerFactory> _logger;

        private readonly ToneAnalyzer.ToneAnalyzerFunction _toneAnalyzerFunction;
        private readonly Mock<IAsyncCollector<FinalTone>> _mockAsyncCollector;

        public ToneAnalyzerFunctionTests()
        {
            _dominantToneMapper = new Mock<IDominantToneStrategy>();
            _toneService = new Mock<IToneService>();
            _jokeService = new Mock<IJokeService>();
            _mockAsyncCollector = new Mock<IAsyncCollector<FinalTone>>();
            _logger = new Mock<ILoggerFactory>();

            _toneAnalyzerFunction = new ToneAnalyzer.ToneAnalyzerFunction(_dominantToneMapper.Object,
                _toneService.Object,
                _logger.Object,
                _jokeService.Object);
        }

        [Fact]
        public async Task SendRequest_HappyComment_DoesNotCallJokeService()
        {
            var request = CreateHttpRequest("this is great");
            var dominantTone = new DominantTone
            {
                IsHappy = true,
                Name = "Joy",
                Score = (decimal)0.75
            };

            _dominantToneMapper.Setup(_ => _.Create(It.IsAny<IEnumerable<Tone>>())).Returns(dominantTone);
            _toneService.Setup(_ => _.GetTonesAsync(It.IsAny<Comment>())).ReturnsAsync(It.IsAny<IEnumerable<Tone>>());

            var result = (OkObjectResult) await _toneAnalyzerFunction.Run(request, _mockAsyncCollector.Object);

            var resultValue = (FinalTone) result.Value;

            Assert.Equal("Joy", resultValue.Name);
            _jokeService.Verify(_ => _.Get(), Times.Never);
        }

        [Fact]
        public async Task SendRequest_SadComment_ReturnSadToneWithJoke()
        {
            var request = CreateHttpRequest("im sad");
            var dominantTone = new DominantTone
            {
                IsSad = true,
                Name = "Sad",
                Score = (decimal)0.75
            };
            var jokes = CreateJokes();

            _dominantToneMapper.Setup(_ => _.Create(It.IsAny<IEnumerable<Tone>>())).Returns(dominantTone);
            _toneService.Setup(_ => _.GetTonesAsync(It.IsAny<Comment>())).ReturnsAsync(It.IsAny<IEnumerable<Tone>>());
            _jokeService.Setup(_ => _.Get()).ReturnsAsync(jokes);

            var result = (OkObjectResult)await _toneAnalyzerFunction.Run(request, _mockAsyncCollector.Object);

            var resultValue = (FinalTone)result.Value;

            Assert.Equal("Sad", resultValue.Name);
            Assert.Equal($"To cheer you up here is a joke: {jokes.First().Setup} {jokes.First().Punchline}", resultValue.Joke);
        }

        [Fact]
        public async Task SendRequest_NeitherHappyOrSadComment_ReturnOtherToneWithJoke()
        {
            var request = CreateHttpRequest("i feel analytical");
            var dominantTone = new DominantTone
            {
                IsOther = true,
                Name = "Analytical",
                Score = (decimal)0.75
            };
            var jokes = CreateJokes();

            CreateSetup(dominantTone);

            _jokeService.Setup(_ => _.Get()).ReturnsAsync(jokes);

            var result = (OkObjectResult) await _toneAnalyzerFunction.Run(request, _mockAsyncCollector.Object);

            var resultValue = (FinalTone) result.Value;

            Assert.Equal("Analytical", resultValue.Name);
            Assert.Equal($"We think you may still fancy a joke: {jokes.First().Setup} {jokes.First().Punchline}", resultValue.Joke);
        }

        [Fact]
        public async Task SendRequest_InvalidText_Throws400StatusCode()
        {
            var request = CreateHttpRequest("");

            var result = (BadRequestObjectResult)await _toneAnalyzerFunction.Run(request, _mockAsyncCollector.Object);

            Assert.Equal(400, result.StatusCode.Value);
        }

        private static List<Joke> CreateJokes()
        {
            var jokes = new List<Joke>
            {
                new Joke
                {
                    Setup = "This is a joke",
                    Punchline = "hahaha"
                }
            };
            return jokes;
        }

        private void CreateSetup(DominantTone dominantTone)
        {
            _dominantToneMapper.Setup(_ => _.Create(It.IsAny<IEnumerable<Tone>>())).Returns(dominantTone);
            _toneService.Setup(_ => _.GetTonesAsync(It.IsAny<Comment>())).ReturnsAsync(It.IsAny<IEnumerable<Tone>>());
        }

        private static HttpRequestMessage CreateHttpRequest(string queryStringValue)
        {
            var request = new HttpRequestMessage();
            var postData = "{\"text\": \"" + $"{queryStringValue}" + "\"}";
            var content = new StringContent(postData, Encoding.UTF8, "application/json");

            request.Content = content;
            request.Method = HttpMethod.Post;

            return request;
        }
    }
}
