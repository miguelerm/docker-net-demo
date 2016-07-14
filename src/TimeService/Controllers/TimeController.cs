using Serilog;
using System;
using System.Web.Http;

namespace TimeService.Controllers
{
    public class TimeController : ApiController
    {
        private readonly ILogger log = Log.ForContext<ApiController>();

        public IHttpActionResult Get()
        {
            log.Debug("Geting Utc Time");
            return Ok(DateTime.UtcNow);
        }
    }
}