using System;

namespace TimeService.Configuration
{
    internal class EnvironmentConfiguration : IConfiguration
    {
        public string Url { get; private set; }

        public EnvironmentConfiguration()
        {
            Url = Environment.GetEnvironmentVariable("TIMESERVICE_URL");
        }
    }
}