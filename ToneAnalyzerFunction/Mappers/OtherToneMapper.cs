using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Mappers;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Services;

namespace ToneAnalyzer.Mappers
{
    public class OtherToneMapper : ToneMapper
    {
        private readonly IJokeService _jokeService;

        public OtherToneMapper(IJokeService jokeService) : base(jokeService)
        {
            _jokeService = jokeService;
        }

        public override async Task<FinalTone> MapAsync(string comment, DominantTone dominantDominantTone)
        {
            var joke = await _jokeService.Get();

            var firstJoke = joke.First();

            var otherTone = new FinalTone
            {
                Comment = comment,
                Name = dominantDominantTone.Name,
                Score = dominantDominantTone.Score,
                Joke = $"We think you may still fancy a joke: {firstJoke.Setup + " " + firstJoke.Punchline}"
            };

            return otherTone;
        }
    }
}
