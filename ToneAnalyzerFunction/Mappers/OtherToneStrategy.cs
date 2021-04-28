using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class OtherToneStrategy : IToneStrategy
    {
        private readonly ToneStrategy _toneStrategy;

        public OtherToneStrategy(ToneStrategy toneStrategy)
        {
            _toneStrategy = toneStrategy;
        }

        public async Task<FinalTone> MapAsync(string comment, DominantTone dominantTone)
        {
            var jokes = await _toneStrategy.JokeService.Get();
            var jokeToSend = jokes.First();
            var finalTone = await _toneStrategy.MapAsync(comment, dominantTone);

            finalTone.Joke = $"We think you may still fancy a joke: {jokeToSend.Setup + " " + jokeToSend.Punchline}";

            return finalTone;
        }
    }
}
