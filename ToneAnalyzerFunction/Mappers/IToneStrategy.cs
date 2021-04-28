using System.Threading.Tasks;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public interface IToneStrategy
    {
        Task<FinalTone> MapAsync(string comment, DominantTone dominantTone);
    }

    public class ToneStrategy : IToneStrategy
    {
        public IJokeService JokeService { get; set; }

        public ToneStrategy(IJokeService jokeService)
        {
            JokeService = jokeService;
        }

        public virtual async Task<FinalTone> MapAsync(string comment, DominantTone dominantTone)
        {
            return await Task.FromResult(new FinalTone
            {
                Comment = comment,
                Name = dominantTone.Name,
                Score = dominantTone.Score
            });
        }
    }
}
