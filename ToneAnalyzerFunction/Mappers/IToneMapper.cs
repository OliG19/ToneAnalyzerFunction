using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;
using ToneAnalyzerFunction.Services;

namespace ToneAnalyzerFunction.Mappers
{
    public interface IToneMapper
    {
        Task<FinalTone> MapAsync(string comment, DominantTone dominantDominantTone);
    }

    public abstract class ToneMapper : IToneMapper
    {
        private readonly IJokeService _jokeService;

        protected ToneMapper(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        public virtual async Task<FinalTone> MapAsync(string comment, DominantTone dominantDominantTone)
        {
            return await Task.FromResult(new FinalTone
            {
                Comment = comment,
                Name = dominantDominantTone.Name,
                Score = dominantDominantTone.Score
            });
        }
    }
}
