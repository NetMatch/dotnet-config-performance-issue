using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace BenchmarkConfig
{
    public class ConfigurationBench
    {
        private IConfigurationSection _configSection;

        [Params("/smallconfig.json", "/largeconfig.json")]
        public string ConfigFile { get; set; }

        [Params(false, true)]
        public bool UseCache { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            IConfigurationSource source = new JsonConfigurationSource { Path = ConfigFile };
            if(UseCache)
            {
                source = new ConfigurationSourceCacheDecorator(source);
            }

            var builder = new ConfigurationBuilder();
            builder.Add(source);
            var rootConfig = builder.Build();
            _configSection = rootConfig.GetSection("MySection:Example");
        }

        [Benchmark]
        public ConfiguredObject GetObjectFromConfiguration()
        {
            return ConfigurationBinder.Get<ConfiguredObject>(_configSection);
        }
    }
}
