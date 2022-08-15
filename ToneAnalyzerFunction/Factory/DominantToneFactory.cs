using System;
using System.Collections.Generic;
using System.Linq;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Factory
{
    public static class DominantToneFactory
    {
        public static DominantTone CreateDominantTone(IEnumerable<Tone> tones)
        {
            var toneDictionary = tones.ToDictionary(tone => tone.ToneName, tone => tone.Score);

            var calculatedDominantTone = toneDictionary.Aggregate((x, y) => x.Value > y.Value ? x : y);

            return new DominantTone
            {
                Name = calculatedDominantTone.Key,
                Score = (decimal)calculatedDominantTone.Value
            };
        }
    }
}
