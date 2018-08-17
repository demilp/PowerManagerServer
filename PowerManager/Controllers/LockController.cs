using Microsoft.Extensions.Options;
using System;
using System.Runtime.InteropServices;
using static PowerManager.Startup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("Lock")]
    public class LockController : Controller
    {
        [DllImport("user32")]
        public static extern void LockWorkStation();

        [HttpPost]
        [EnableCors("AllowAll")]
        public Response Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    LockWorkStation();
                    return new Response { success = false, data = "", error = "" };
                }
                catch (Exception e) { return new Response { success = false, data = "", error = e.Message }; }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }
        private IConfiguration configuration;
        public LockController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
    }
}
