using System.Threading.Tasks;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public interface IToneMapper
    {
        Task<FinalTone> MapAsync(string comment, DominantTone dominantTone);
    }

    public abstract class ToneMapper : IToneMapper
    {
        protected ToneMapper()
        { }

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
