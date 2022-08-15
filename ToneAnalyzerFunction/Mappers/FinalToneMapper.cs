using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public static class FinalToneMapper
    {
        public static FinalTone MapAsync(string comment, DominantTone dominantTone)
        {
            return new FinalTone
            {
                Comment = comment,
                Name = dominantTone.Name,
                Score = dominantTone.Score
            };
        }
    }
}
