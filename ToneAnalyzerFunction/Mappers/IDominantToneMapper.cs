using System;
using System.Collections.Generic;
using System.Text;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzerFunction.Mappers
{
    public interface IDominantToneMapper
    {
        DominantTone Create(IEnumerable<Tone> tones);
    }
}
