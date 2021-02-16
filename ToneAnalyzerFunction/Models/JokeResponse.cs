using System;
using System.Collections.Generic;
using System.Text;

namespace ToneAnalyzerFunction.Models
{
    public class JokeResponse
    {
        public IEnumerable<Joke> Body { get; set; }
    }

    public class Joke
    {
        public string Setup { get; set; }

        public string Punchline { get; set; }
    }
}
