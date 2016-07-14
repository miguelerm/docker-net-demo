using Microsoft.Owin.Logging;
using Owin;
using System.Web.Http;
using System.Web.Http.Tracing;
using TimeService.Logging;
using LoggerFactory = TimeService.Logging.LoggerFactory;

namespace TimeService
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSerilogRequestContext();

            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Services.Replace(typeof(ITraceWriter), new SerilogTraceWriter());

            app.UseWebApi(config);

            const string LoggerFactoryAppKey = "server.LoggerFactory";

            app.SetLoggerFactory(new LoggerFactory());
        }
    }
}