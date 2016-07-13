using Serilog;
using TimeService.Configuration;
using Topshelf;

namespace TimeService
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            HostFactory.Run(x =>
            {
                x.SelectPlatform();

                x.UseSerilog();

                var commandLineConfiguration = new CommandLineConfiguration();

                x.AddCommandLineDefinition("url", commandLineConfiguration.SetUrl);
                x.ApplyCommandLine();

                var config = new MultipleSourcesConfiguration(new IConfiguration[] {
                    commandLineConfiguration,
                    new ApplicationConfiguration(),
                    new EnvironmentConfiguration(),
                    // new RemoteConfiguration(),
                    new DefaultConfiguration()
                });

                x.Service<Daemon>(s =>
                {
                    s.ConstructUsing(name => new Daemon(config));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("SelfHosted TimeService");
                x.SetDisplayName("SelfHosted TimeService");
                x.SetServiceName("SelfHosted.TimeService");
            });
        }
    }
}
