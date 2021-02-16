using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzerFunction.Mappers
{
    public class OtherToneMapper : ToneMapper
    {
        public override async Task<FinalTone> MapAsync(string comment, DominantTone dominantDominantTone)
        {
            var joke = await dominantDominantTone.GetJokeService().Get();
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
