using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using static System.Net.Mime.MediaTypeNames;
using RouteAttribute = System.Web.Http.RouteAttribute;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightMobileApp
{
    [Route("screenshot")]
    public class ScreenController : Controller
    {
        [Route("")]
        // GET: /screenshot
        public IActionResult Getscreentshot()
        {
            string url = "http://127.0.0.1:8080/screenshot";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResponse = myRequest.GetResponse();
            MemoryStream ms = new MemoryStream();
            myResponse.GetResponseStream().CopyTo(ms);
            byte[] data = ms.ToArray();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(File(data, "image/jpeg"));
        }
    }
}
