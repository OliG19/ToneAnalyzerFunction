using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class OtherToneStrategy : IToneStrategy
    {
        private readonly IJokeService _jokeService;

        public OtherToneStrategy(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        public async Task<FinalTone> SetFinalToneJoke(FinalTone finalTone)
        {
            var joke = await _jokeService.GetAsync();;

            finalTone.Joke = $"We think you may still fancy a joke: {joke.Setup + " " + joke.Punchline}";

            return finalTone;
        }
    }
}
