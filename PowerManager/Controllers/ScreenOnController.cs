using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static PowerManager.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("ScreenOn")]
    public class ScreenOnController : Controller
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        [HttpPost]
        [EnableCors("AllowAll")]
        public Response Post([FromBody]string value)
        {
            if (value == configuration.GetSection("Settings").GetSection("password").Value)
            {
                try
                {
                    //Process.Start("powershell(Add-Type '[DllImport(\"user32.dll\")]^public static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);' - Name a - Pas)::SendMessage(-1, 0x0112, 0xF170, -1)");
                    //SendMessage(0, 0x0112, 0xF170, -1);
                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "TurnOnScreen.bat");
                    return new Response { success = true, data = "", error = "" };
                }
                catch (Exception e) {
                    return new Response { success = false, data = "", error = e.Message };
                }
            }
            return new Response { success = false, data = "", error = "Incorrect password" };
        }
        private IConfiguration configuration;
        public ScreenOnController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        /*private string RunScript(string scriptText)
        {
            // create Powershell runspace

            Runspace runspace = RunspaceFactory.CreateRunspace();

            // open it

            runspace.Open();

            // create a pipeline and feed it the script text

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // add an extra command to transform the script
            // output objects into nicely formatted strings

            // remove this line to get the actual objects
            // that the script returns. For example, the script

            // "Get-Process" returns a collection
            // of System.Diagnostics.Process instances.

            pipeline.Commands.Add("Out-String");

            // execute the script

            Collection<PSObject> results = pipeline.Invoke();

            // close the runspace

            runspace.Close();

            // convert the script result into a single string

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            return stringBuilder.ToString();
        }*/
    }
}
