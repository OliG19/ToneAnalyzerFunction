using System.Threading.Tasks;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class HappyToneMapper : ToneMapper
    {
        public override async Task<FinalTone> MapAsync(string comment, DominantTone dominantTone)
        {
            var finalTone = await base.MapAsync(comment, dominantTone);

            finalTone.Joke = "You don't need a joke, you're happy enough!";

            return finalTone;
        }
    }
}
