using System;
using System.Collections.Generic;
using System.Text;

namespace ToneAnalyzerFunction.Models.Configuration
{
    public static class WatsonConfiguration
    {
        public static string WatsonApiKey => Environment.GetEnvironmentVariable("WatsonApiKey");

        public static string WatsonUrl => Environment.GetEnvironmentVariable("WatsonUrl");
    }
}
