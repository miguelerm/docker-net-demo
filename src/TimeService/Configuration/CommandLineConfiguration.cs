namespace TimeService.Configuration
{
    internal class CommandLineConfiguration : IConfiguration
    {
        public string Url { get; private set; }

        internal void SetUrl(string url)
        {
            this.Url = url;
        }
    }
}