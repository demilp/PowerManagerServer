using System;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using static PowerManager.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("Restart")]
    public class RestartController : Controller
    {
        [HttpPost]
        [EnableCors("AllowAll")]
        public Response Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    Process.Start("shutdown", "/r /t 0");
                    return new Response { success = true, data = "", error = "" };
                }
                catch (Exception e) { return new Response { success = false, data = "", error = e.Message }; }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }
        private IConfiguration configuration;
        public RestartController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
    }
}
