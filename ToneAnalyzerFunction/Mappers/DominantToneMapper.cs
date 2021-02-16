using System.Collections.Generic;
using System.Linq;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzerFunction.Mappers
{
    public class DominantToneMapper : IDominantToneMapper
    {
        public DominantTone Create(IEnumerable<Tone> tones)
        {
            var toneDictionary = tones.ToDictionary(tone => tone.ToneName, tone => tone.Score);

            var dominantToneValuePair = toneDictionary.Aggregate((x, y) => x.Value > y.Value ? x : y);

            var dominantTone = new DominantTone
            {
                Score = (decimal) dominantToneValuePair.Value,
                Name = dominantToneValuePair.Key
            };

            switch (dominantToneValuePair.Key)
            {
                case "Joy":
                    dominantTone.IsHappy = true;
                    break;
                case "Sad":
                    dominantTone.IsSad = true;
                    break;
                default:
                    dominantTone.IsOther = true;
                    break;
            }

            return dominantTone;
        }
    }
}
