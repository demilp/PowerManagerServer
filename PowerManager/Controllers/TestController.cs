using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static PowerManager.Startup;

namespace PowerManager.Controllers
{
    public class TestController : Controller
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        [Produces("application/json")]
        [Route("Test")]
        public string Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    //Process.Start("powershell (Add-Type '[DllImport(\"user32.dll\")]^public static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);' -Name a -Pas)::SendMessage(-1,0x0112,0xF170,2)");
                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "TurnOffScreen.bat");
                    //SendMessage(-1, 0x0112, 0xF170, 2);
                    return "true";
                }
                catch (Exception e)
                {
                    return e.Message + " "+ AppDomain.CurrentDomain.BaseDirectory;
                }
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        private IConfiguration configuration;
        public TestController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
    }
}
