using System.Collections.Generic;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public interface IDominantToneStrategy
    {
        DominantTone Create(IEnumerable<Tone> tones);
    }
}
