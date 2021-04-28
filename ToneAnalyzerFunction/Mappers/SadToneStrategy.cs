using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class SadToneStrategy : IToneStrategy
    {
        private readonly ToneStrategy _toneStrategy;

        public SadToneStrategy(ToneStrategy toneStrategy)
        {
            _toneStrategy = toneStrategy;
        }

        public async Task<FinalTone> MapAsync(string comment, DominantTone dominantTone)
        {
            var jokes = await _toneStrategy.JokeService.Get();
            var jokeToSend = jokes.First();
            var finalTone = await _toneStrategy.MapAsync(comment, dominantTone);

            finalTone.Joke = $"To cheer you up here is a joke: {jokeToSend.Setup + " " + jokeToSend.Punchline}";

            return finalTone;
        }
    }
}
