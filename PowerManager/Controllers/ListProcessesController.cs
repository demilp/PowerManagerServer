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
    [Route("ListProcesses")]
    public class ListProcessesController : Controller
    {
        [HttpPost]
        [EnableCors("AllowAll")]
        public Response Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    var allProcesses = Process.GetProcesses();
                    var d = allProcesses.Where(p => { return p.MainWindowHandle != new IntPtr(0); }).Select(p => {
                        try
                        {
                            var f = p.MainModule.FileVersionInfo.FileDescription;
                            if (f != "Windows Shell Experience Host") {
                                return new { processName = f, pid = p.Id, valid = true };
                            }
                            return new { processName = f, pid = p.Id, valid = false };
                        }
                        catch (Exception) {
                            return new { processName = p.ProcessName, pid = p.Id, valid = false }; ;
                        }
                    });//.Where(x=>x!=null );
                    return new Response { success = false, data = d, error = "" };
                }
                catch (Exception e) { return new Response { success = false, data = "", error = e.Message }; }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }

        private IConfiguration configuration;
        public ListProcessesController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
    }
}