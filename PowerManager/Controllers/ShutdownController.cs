using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using static PowerManager.Startup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("Shutdown")]
    public class ShutdownController : Controller
    {
        
        [HttpPost]
        [EnableCors("AllowAll")]
        // POST: api/Shutdown
        public Response Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    Process.Start("shutdown", "/s /t 0");
                    return new Response { success = true, data = "", error = "" };
                }
                catch (Exception e) { return new Response { success = false, data = "", error = e.Message }; }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }
        private IConfiguration configuration;
        public ShutdownController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

    }
}
