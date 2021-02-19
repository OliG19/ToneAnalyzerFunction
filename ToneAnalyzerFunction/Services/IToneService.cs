using System.Collections.Generic;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Services
{
    public interface IToneService
    {
        Task<IEnumerable<Tone>> GetTonesAsync(Comment comment);
    }
}
