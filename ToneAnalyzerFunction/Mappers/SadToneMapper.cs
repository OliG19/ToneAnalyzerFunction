using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Mappers;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Services;

namespace ToneAnalyzer.Mappers
{
    public class SadToneMapper : ToneMapper
    {
        private readonly IJokeService _jokeService;

        public SadToneMapper(IJokeService jokeService) : base(jokeService)
        {
            _jokeService = jokeService;
        }

        public override async Task<FinalTone> MapAsync(string comment, DominantTone dominantTone)
        {
            var jokes = await _jokeService.Get();
            var jokeToSend = jokes.First();

            var sadTone = new FinalTone
            {
                Comment = comment,
                Name = dominantTone.Name,
                Score = dominantTone.Score,
                Joke = $"To cheer you up here is a joke: {jokeToSend.Setup + " " + jokeToSend.Punchline}"
            };

            return sadTone;
        }
    }
}
