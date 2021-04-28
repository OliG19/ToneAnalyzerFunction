using System.IO;
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

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
        }

        private void ConfigureServices(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            builder.Services.AddSingleton<IToneService, ToneService>();
            builder.Services.AddSingleton<IJokeService, JokeService>();
            builder.Services.AddSingleton<IDominantToneStrategy, DominantToneStrategy>();
        }
    }
}
