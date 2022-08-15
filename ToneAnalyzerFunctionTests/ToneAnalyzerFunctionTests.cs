using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToneAnalyzer.Factory;
using ToneAnalyzer.Mappers;
using ToneAnalyzer.Models;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;
using Xunit;

namespace ToneAnalyzerTests
{
    public class ToneAnalyzerFunctionTests
    {
        private readonly Mock<IToneStrategyFactory> _toneStrategyFactoryMock;
        private readonly Mock<IToneService> _toneServiceMock;
        private readonly Mock<IJokeService> _jokeServiceMock;
        private readonly Mock<ILoggerFactory> _logger;

        private readonly ToneAnalyzer.ToneAnalyzerFunction _toneAnalyzerFunction;
        private readonly Mock<IAsyncCollector<FinalTone>> _mockAsyncCollector;

        public ToneAnalyzerFunctionTests()
        {
            _toneStrategyFactoryMock = new Mock<IToneStrategyFactory>();
            _toneServiceMock = new Mock<IToneService>();
            _jokeServiceMock = new Mock<IJokeService>();
            _mockAsyncCollector = new Mock<IAsyncCollector<FinalTone>>();
            _logger = new Mock<ILoggerFactory>();

            _toneAnalyzerFunction = new ToneAnalyzer.ToneAnalyzerFunction(
                _toneStrategyFactoryMock.Object,
                _toneServiceMock.Object,
                _logger.Object);
        }

        [Fact]
        public async Task SendRequest_HappyComment_DoesNotCallJokeService()
        {
            var request = CreateHttpRequest("this is great");
            var toneName = "Joy";
            var tones = new List<Tone>
            {
                new Tone
                {
                    ToneName = "Other",
                    ToneId = "Other",
                    Score = 0.25
                },
                new Tone
                {
                    ToneName = "Sad",
                    ToneId = "Other",
                    Score = 0.25
                },
                new Tone
                {
                    ToneName = toneName,
                    ToneId = "Other",
                    Score = 0.50
                }
            };

            _toneServiceMock.Setup(_ => _.GetTonesAsync(It.IsAny<Comment>())).ReturnsAsync(tones);
            _toneStrategyFactoryMock.Setup(_ => _.Create(toneName)).Returns(new HappyToneStrategy());

            var result = (OkObjectResult)await _toneAnalyzerFunction.Run(request, _mockAsyncCollector.Object);

            var resultValue = (FinalTone)result.Value;

            resultValue.Name.Should().Be(toneName);
            resultValue.Joke.Should().Be("You don't need a joke, you're happy enough!");
            _jokeServiceMock.Verify(_ => _.GetAsync(), Times.Never);
        }

        [Fact]
        public async Task SendRequest_SadComment_ReturnSadToneWithJoke()
        {
            var request = CreateHttpRequest("im sad");
            var toneName = "Sad";
            var tones = new List<Tone>
            {
                new Tone
                {
                    ToneName = "Other",
                    ToneId = "Other",
                    Score = 0.25
                },
                new Tone
                {
                    ToneName = toneName,
                    ToneId = "Sad",
                    Score = 0.50
                },
                new Tone
                {
                    ToneName = "Happy",
                    ToneId = "Happy",
                    Score = 0.25
                }
            };
            var joke = CreateJoke();

            _toneServiceMock.Setup(_ => _.GetTonesAsync(It.IsAny<Comment>())).ReturnsAsync(tones);
            _toneStrategyFactoryMock.Setup(_ => _.Create(toneName)).Returns(new SadToneStrategy(_jokeServiceMock.Object));
            _jokeServiceMock.Setup(_ => _.GetAsync()).ReturnsAsync(joke);
            var result = (OkObjectResult)await _toneAnalyzerFunction.Run(request, _mockAsyncCollector.Object);

            var resultValue = (FinalTone)result.Value;

            resultValue.Name.Should().Be(toneName);
            var expectedJoke = $"To cheer you up here is a joke: {joke.Setup} {joke.Punchline}";
            resultValue.Joke.Should().Be(expectedJoke);
        }

        [Fact]
        public async Task SendRequest_NeitherHappyOrSadComment_ReturnOtherToneWithJoke()
        {
            var comment = "i feel grumpy";
            var request = CreateHttpRequest(comment);
            var toneName = "Other";
            var tones = new List<Tone>
            {
                new Tone
                {
                    ToneName = toneName,
                    ToneId = "Other",
                    Score = 0.50
                },
                new Tone
                {
                    ToneName = "Sad",
                    ToneId = "Other",
                    Score = 0.25
                },
                new Tone
                {
                    ToneName = "Happy",
                    ToneId = "Other",
                    Score = 0.25
                }
            };
            var joke = CreateJoke();

            _toneStrategyFactoryMock.Setup(_ => _.Create(toneName)).Returns(new OtherToneStrategy(_jokeServiceMock.Object));
            _toneServiceMock.Setup(_ => _.GetTonesAsync(It.IsAny<Comment>())).ReturnsAsync(tones);
            _jokeServiceMock.Setup(_ => _.GetAsync()).ReturnsAsync(joke);

            var result = (OkObjectResult)await _toneAnalyzerFunction.Run(request, _mockAsyncCollector.Object);

            var resultValue = (FinalTone)result.Value;

            Assert.Equal("Other", resultValue.Name);
            Assert.Equal($"We think you may still fancy a joke: {joke.Setup} {joke.Punchline}", resultValue.Joke);
        }

        [Fact]
        public async Task SendRequest_InvalidText_Throws400StatusCode()
        {
            var request = CreateHttpRequest("");

            var result = (BadRequestObjectResult)await _toneAnalyzerFunction.Run(request, _mockAsyncCollector.Object);

            Assert.Equal(400, result.StatusCode.Value);
        }

        private static Joke CreateJoke()
        {
            return new Joke
            {
                Setup = "This is a joke",
                Punchline = "hahaha"
            };
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
