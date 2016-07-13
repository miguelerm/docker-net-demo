using System.Collections.Generic;
using System.Linq;

namespace TimeService.Configuration
{
    internal class MultipleSourcesConfiguration : IConfiguration
    {
        public string Url { get; private set; }

        public MultipleSourcesConfiguration(IEnumerable<IConfiguration> configurations = null)
        {
            if (configurations != null && configurations.Any())
            {
                foreach (var config in configurations)
                {
                    CopyMissingValues(config);
                }
            }
        }

        private void CopyMissingValues(IConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(Url))
                Url = config.Url;
        }
    }
}