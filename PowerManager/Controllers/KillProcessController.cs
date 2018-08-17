using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("KillProcess")]
    public class KillProcessController : Controller
    {
        [HttpPost("{id}")]
        [EnableCors("AllowAll")]
        public Response Post(int id, [FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    Process p = Process.GetProcessById(id);
                    p.Kill();
                    return new Response { success = true, data = "", error = "" };
                }
                catch (Exception e) { return new Response { success = false, data = "", error = e.Message }; }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }
        private IConfiguration configuration;
        public KillProcessController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

    }
}