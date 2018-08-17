using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using static PowerManager.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("Hibernate")]
    public class HibernateController : Controller
    {
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        [HttpPost]
        [EnableCors("AllowAll")]
        public Response Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    SetSuspendState(true, true, true);
                    return new Response { success = false, data = "", error = "" };
                }
                catch (Exception e) { return new Response { success = false, data = "", error = e.Message }; }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }
        private IConfiguration configuration;
        public HibernateController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
    }
}
