using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TimeService.Controllers
{
    public class TimeController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(DateTime.UtcNow);
        }
    }
}
