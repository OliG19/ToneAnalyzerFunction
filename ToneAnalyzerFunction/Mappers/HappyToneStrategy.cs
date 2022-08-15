using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class HappyToneStrategy : IToneStrategy
    {
        public async Task<FinalTone> SetFinalToneJoke(FinalTone finalTone)
        {
            finalTone.Joke = "You don't need a joke, you're happy enough!";

            return finalTone;
        }
    }
}
