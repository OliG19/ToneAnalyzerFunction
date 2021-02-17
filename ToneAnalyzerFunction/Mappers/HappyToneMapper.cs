using ToneAnalyzerFunction.Mappers;
using ToneAnalyzerFunction.Services;

namespace ToneAnalyzer.Mappers
{
    public class HappyToneMapper : ToneMapper
    {
        public HappyToneMapper(IJokeService jokeService) : base(jokeService)
        { }
    }
}
