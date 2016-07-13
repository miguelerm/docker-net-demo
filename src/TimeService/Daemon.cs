using Microsoft.Owin.Hosting;
using Serilog;
using System;
using TimeService.Configuration;

namespace TimeService
{
    internal class Daemon
    {
        private readonly IConfiguration configuration;
        private IDisposable app;
        private readonly ILogger log = Log.ForContext<Daemon>();

        public Daemon(IConfiguration configuration)
        {
            this.configuration = configuration ?? new DefaultConfiguration();
        }

        public void Start()
        {
            var url = configuration.Url;
            app = WebApp.Start<Startup>(url);
            log.Information("Service listening on {url}", url);
        }

        public void Stop()
        {
            app.Dispose();
            log.Information("Service Stopped");
        }
    }
}