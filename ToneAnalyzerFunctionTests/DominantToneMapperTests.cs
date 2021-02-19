using System.Collections.Generic;
using ToneAnalyzer.Mappers;
using ToneAnalyzerFunction.Models;
using Xunit;

namespace ToneAnalyzerTests
{
    public class DominantToneMapperTests
    {
        [Fact]
        public void Create_ValidToneList_CreatesCorrectDominantTone()
        {
            var tones = new List<Tone>
            {
                new Tone
                {
                    Score = 0.75,
                    ToneName = "Joy"
                },
                new Tone
                {
                    Score = 0.85,
                    ToneName = "Sad"
                },
                new Tone
                {
                    Score = 0.95,
                    ToneName = "Fear"
                },
            };
            var dominantToneMapper = new DominantToneMapper();

            var result = dominantToneMapper.Create(tones);

            Assert.Equal((decimal)0.95, result.Score);
            Assert.Equal("Fear", result.Name);
            Assert.True(result.IsOther);
        }
    }
}
