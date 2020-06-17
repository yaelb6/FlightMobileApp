using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightMobileApp
{
    [Route("screenshot")]
    public class ScreenController : Controller
    {
        [Route("")]
        // GET: /screenshot
        public ActionResult Getscreentshot()
        {
            string url = "http://127.0.0.1:8080/screenshot";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResponse = myRequest.GetResponse();
            MemoryStream ms = new MemoryStream();
            myResponse.GetResponseStream().CopyTo(ms);
            byte[] data = ms.ToArray();
            return File(data, "image/jpeg");
        }
    }
}
