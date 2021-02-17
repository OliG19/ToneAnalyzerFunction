using System;

namespace ToneAnalyzer.Models.Configuration
{
    public static class JokeConfiguration
    {
        public static string JokeUrl => Environment.GetEnvironmentVariable("JokeUrl");

        public static string JokeApikey => Environment.GetEnvironmentVariable("JokeApikey");

        public static string JokeApiHost => Environment.GetEnvironmentVariable("JokeApiHost");
    }
}
