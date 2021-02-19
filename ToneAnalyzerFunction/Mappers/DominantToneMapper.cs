using System.Collections.Generic;
using System.Linq;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public class DominantToneMapper : IDominantToneMapper
    {
        public DominantTone Create(IEnumerable<Tone> tones)
        {
            var (name, score) = GetDominantToneValuePair(tones);

            var dominantTone = new DominantTone
            {
                Name = name,
                Score = (decimal) score
            };

            switch (name)
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

        private static KeyValuePair<string, double> GetDominantToneValuePair(IEnumerable<Tone> tones)
        {
            var toneDictionary = tones.ToDictionary(tone => tone.ToneName, tone => tone.Score);

            return toneDictionary.Aggregate((x, y) => x.Value > y.Value ? x : y);
        }
    }
}
