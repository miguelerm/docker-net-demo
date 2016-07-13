namespace TimeService.Configuration
{
    internal class DefaultConfiguration : IConfiguration
    {
        private const string defaultUrl = "http://localhost:8080";

        public string Url { get { return defaultUrl; } }
    }
}