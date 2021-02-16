using System;
using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzerFunction.Mappers
{
    public class SadToneMapper : ToneMapper
    {
        public override async Task<FinalTone> MapAsync(string comment, DominantTone dominantTone)
        {
            var joke = await dominantTone.GetJokeService().Get();
            var random = new Random();
            var x = random.Next(joke.Count());
            var firstJoke = joke.First();
            var sadTone = new FinalTone
            {
                Comment = comment,
                Name = dominantTone.Name,
                Score = dominantTone.Score,
                Joke = $"To cheer you up here is a joke: {firstJoke.Setup + " " + firstJoke.Punchline}"
            };

            return sadTone;
        }
    }
}
