using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToneAnalyzer.Mappers;
using ToneAnalyzer.Services;
using ToneAnalyzerFunction;
using ToneAnalyzerFunction.Models.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ToneAnalyzerFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder);
        }

        private void ConfigureServices(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            builder.Services.AddSingleton<IToneService, ToneService>();
            builder.Services.AddSingleton<IJokeService, JokeService>();
            builder.Services.AddSingleton<IDominantToneMapper, DominantToneMapper>();
        }
    }
}
