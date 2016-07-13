using Owin;
using System.Web.Http;

namespace TimeService
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            app.UseWebApi(config);
        }
    }
}