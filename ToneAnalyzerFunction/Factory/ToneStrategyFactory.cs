using System.Collections.Generic;
using System.Linq;
using ToneAnalyzer.Mappers;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction.Models;

namespace ToneAnalyzer.Factory
{
    public class ToneStrategyFactory : IToneStrategyFactory
    {
        private readonly IJokeService _jokeService;

        public ToneStrategyFactory(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        public IToneStrategy Create(string toneName)
        {
            return toneName switch
            {
                "Joy" => new HappyToneStrategy(),
                "Sad" => new SadToneStrategy(_jokeService),
                _ => new OtherToneStrategy(_jokeService),
            };
        }
    }
}
