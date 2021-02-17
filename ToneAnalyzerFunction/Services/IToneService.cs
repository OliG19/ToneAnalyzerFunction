using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzerFunction.Services
{
    public interface IToneService
    {
        Task<IEnumerable<Tone>> GetTonesAsync(Comment comment);
    }
}
