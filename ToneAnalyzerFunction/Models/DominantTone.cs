using ToneAnalyzer.Mappers;
using ToneAnalyzerFunction.Mappers;
using ToneAnalyzerFunction.Services;

namespace ToneAnalyzerFunction.Models
{
    public class DominantTone
    {
        public string Name { get; set; }

        public bool IsHappy { get; set; }

        public bool IsSad { get; set; }

        public bool IsOther { get; set; }

        public IToneMapper DominantToneMapper(IJokeService jokeService)
        {
            if (IsHappy)
            {
                return new HappyToneMapper(jokeService);
            }

            if (IsSad)
            {
                return new SadToneMapper(jokeService);
            }

            return new OtherToneMapper(jokeService);
        }

        public decimal Score { get; set; }
    }
}
