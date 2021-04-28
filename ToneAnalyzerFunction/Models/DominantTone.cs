using ToneAnalyzer.Mappers;
using ToneAnalyzer.Services;

namespace ToneAnalyzerFunction.Models
{
    public class DominantTone
    {
        public string Name { get; set; }

        public bool IsHappy { get; set; }

        public bool IsSad { get; set; }

        public bool IsOther { get; set; }

        public decimal Score { get; set; }

        public IToneStrategy DominantToneMapper(IJokeService jokeService)
        {
            var toneMapper = new ToneStrategy(jokeService);

            if (IsHappy)
            {
                return new HappyToneStrategy(toneMapper);
            }

            if (IsSad)
            {
                return new SadToneStrategy(toneMapper);
            }

            return new OtherToneStrategy(toneMapper);
        }
    }
}
