using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class HappyToneStrategy : IToneStrategy
    {
        private readonly ToneStrategy _toneStrategy;

        public HappyToneStrategy(ToneStrategy toneStrategy)
        {
            _toneStrategy = toneStrategy;
        }

        public async Task<FinalTone> MapAsync(string comment, DominantTone dominantTone)
        {
            var finalTone = await _toneStrategy.MapAsync(comment, dominantTone);

            finalTone.Joke = "You don't need a joke, you're happy enough!";

            return finalTone;
        }
    }
}
