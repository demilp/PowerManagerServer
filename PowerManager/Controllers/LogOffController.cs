using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using static PowerManager.Startup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("LogOff")]
    public class LogOffController : Controller
    {
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [HttpPost]
        [EnableCors("AllowAll")]
        public Response Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    ExitWindowsEx(0, 0);
                    return new Response { success = false, data = "", error = "" };
                }
                catch (Exception e) { return new Response { success = false, data = "", error = e.Message }; }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }
        private IConfiguration configuration;
        public LogOffController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
    }
}
