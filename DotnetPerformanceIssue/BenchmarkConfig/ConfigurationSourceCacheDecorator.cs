using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;

namespace BenchmarkConfig
{
    internal class ConfigurationSourceCacheDecorator : IConfigurationSource, IConfigurationProvider
    {
        private readonly IConfigurationSource _configurationSource;
        private IConfigurationProvider _base;

        private readonly ConcurrentDictionary<string, IEnumerable<string>> _cache = new ConcurrentDictionary<string, IEnumerable<string>>();

        public ConfigurationSourceCacheDecorator(IConfigurationSource inner)
        {
            _configurationSource = inner;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            _base = _configurationSource.Build(builder);
            return this;
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            var result = _cache.GetOrAdd(
                parentPath,
                parentPath => _base
                    .GetChildKeys(Enumerable.Empty<string>(), parentPath)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(k => k, ConfigurationKeyComparer.Instance)
                    .ToList());

            return result
                .Concat(earlierKeys)
                .OrderBy(k => k, ConfigurationKeyComparer.Instance);
        }

        public IChangeToken GetReloadToken()
        {
            return _base.GetReloadToken();
        }

        public void Load()
        {
            _base.Load();
            _cache.Clear();
        }

        public void Set(string key, string value)
        {
            _base.Set(key, value);
        }

        public bool TryGet(string key, out string value)
        {
            return _base.TryGet(key, out value);
        }
    }
}
