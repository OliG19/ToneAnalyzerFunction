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

        public IToneMapper DominantToneMapper
        {
            get
            {
                if (IsHappy)
                {
                    return new HappyToneMapper();
                }

                if (IsSad)
                {
                    return new SadToneMapper();
                }

                return new OtherToneMapper();
            }
        }

        public decimal Score { get; set; }

        public JokeService GetJokeService()
            => new JokeService();

    }
}
