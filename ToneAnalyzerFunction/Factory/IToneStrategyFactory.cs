using System.Collections.Generic;
using ToneAnalyzer.Mappers;

namespace ToneAnalyzer.Factory
{
    public interface IToneStrategyFactory
    {
        IToneStrategy Create(string toneName);
    }
}
