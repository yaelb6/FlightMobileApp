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
        public IActionResult Getscreentshot()
        {
            Byte[] b = System.IO.File.ReadAllBytes(@"E:\\Test.jpg");    
            return File(b, "image/jpeg");
        }
    }
}
