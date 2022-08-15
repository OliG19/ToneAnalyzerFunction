using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Mappers
{
    public interface IToneStrategy
    {
        Task<FinalTone> SetFinalToneJoke(FinalTone finalTone);
    }
}
