using System;
using System.Collections.Generic;
using System.Text;

namespace ToneAnalyzerFunction.Models
{
    public class ToneResponse
    {
        public DocumentTones DocumentTone { get; set; }
    }

    public class DocumentTones
    {
        public List<Tone> Tones { get; set; }
    }

    public class Tone
    {
        public double Score { get; set; }
        public string ToneId { get; set; }
        public string ToneName { get; set; }
    }
}
