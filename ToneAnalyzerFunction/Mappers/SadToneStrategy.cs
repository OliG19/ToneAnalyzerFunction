using System.Linq;
using System.Threading.Tasks;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class SadToneStrategy : IToneStrategy
    {
        private readonly IJokeService _jokeService;

        public SadToneStrategy(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        public async Task<FinalTone> SetFinalToneJoke(FinalTone finalTone)
        {
            var joke = await _jokeService.GetAsync();

            finalTone.Joke = $"To cheer you up here is a joke: {joke.Setup + " " + joke.Punchline}";

            return finalTone;
        }
    }
}
