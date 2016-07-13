using System.Configuration;

namespace TimeService.Configuration
{
    internal class ApplicationConfiguration : IConfiguration
    {
        public string Url { get; private set; }

        public ApplicationConfiguration()
        {
            Url = ConfigurationManager.AppSettings["timeservice:url"];
        }
    }
}