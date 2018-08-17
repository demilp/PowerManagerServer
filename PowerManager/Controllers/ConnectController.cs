using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static PowerManager.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using System.Net.NetworkInformation;
using System.Net;

namespace PowerManager.Controllers
{
    [Produces("application/json")]
    [Route("Connect")]
    public class ConnectController : Controller
    {
        // POST: api/Connect

        [HttpPost]
        [EnableCors("AllowAll")]
        public Response Post([FromBody]string value)
        {
            var ip = GetIPAddress();
            var mac = GetMacFromIp(ip[0]);
            return new Response { success = value == configuration.GetSection("Settings").GetSection("password").Value, data = new { mac=mac, ip=ip[0], hostname=ip[1]}, error = "" };
        }
        private IConfiguration configuration;
        public ConnectController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        public static string GetMacFromIp(string ip)
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            var m = nics.Where(nic => (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) && nic.GetIPProperties().UnicastAddresses.Where(i => i.Address.ToString() == ip).Any()).ToArray();
            if (m.Length > 0) {
                var output = string.Join(":", Enumerable.Range(0, 6).Select(i => m[0].GetPhysicalAddress().ToString().Substring(i * 2, 2)));
                return output;
            }
            return "";
            
            foreach (NetworkInterface adapter in nics)
            {
                PhysicalAddress address = adapter.GetPhysicalAddress();
                byte[] bytes = address.GetAddressBytes();
                string mac = null;
                for (int i = 0; i < bytes.Length; i++)
                {
                    mac = string.Concat(mac + (string.Format("{0}", bytes[i].ToString("X2"))));
                    if (i != bytes.Length - 1)
                    {
                        mac = string.Concat(mac + "-");
                    }
                }

                return mac;
                //info += mac + "\n";

                //info += "\n";
            }
            //Debug.Log(info);
            return "";
        }
        public static string[] GetIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList.Where(ip => { return ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork; }).First();
            return new string[] { ipAddress.ToString(), ipHostInfo.HostName };
        }

    }
    
}
