using System.Collections.Generic;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public interface IDominantToneMapper
    {
        DominantTone Create(IEnumerable<Tone> tones);
    }
}
