using System;
using System.Collections.Generic;
using System.Text;

namespace ToneAnalyzerFunction.Models.Configuration
{
    public class WatsonConfiguration
    {
        public string WatsonApiKey => Environment.GetEnvironmentVariable("WatsonApiKey");

        public string WatsonUrl => Environment.GetEnvironmentVariable("WatsonUrl");
    }
}
