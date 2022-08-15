using System.Collections.Generic;
using System.Threading.Tasks;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Services
{
    public interface IJokeService
    {
        Task<Joke> GetAsync();
    }
}
