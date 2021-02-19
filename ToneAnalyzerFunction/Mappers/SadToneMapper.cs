using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class SadToneMapper : ToneMapper
    {
        private readonly IJokeService _jokeService;

        public SadToneMapper(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        public override async Task<FinalTone> MapAsync(string comment, DominantTone dominantTone)
        {
            var jokes = await _jokeService.Get();
            var jokeToSend = jokes.First();
            var finalTone = await base.MapAsync(comment, dominantTone);

            finalTone.Joke = $"To cheer you up here is a joke: {jokeToSend.Setup + " " + jokeToSend.Punchline}";

            return finalTone;
        }
    }
}
