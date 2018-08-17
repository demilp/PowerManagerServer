using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static PowerManager.Startup;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("Sleep")]
    public class SleepController : Controller
    {
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
        // POST: Sleep
        [HttpPost]
        [EnableCors("AllowAll")]
        public Response Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    SetSuspendState(false, true, true);
                    return new Response { success = true, data = "", error = "" };
                }
                catch (Exception e) { return new Response { success = false, data = "", error = e.Message }; }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }
        private IConfiguration configuration;
        public SleepController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

    }
}
