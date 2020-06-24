using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;
using Microsoft.Extensions.Hosting.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace FlightMobileApp
{
    public class CommandController : Controller
    {
        private SimulatorClient telnetClient;
        public Command cmd { get; set; }

        public CommandController(SimulatorClient client)
        {
            this.telnetClient = client;
        }

        [Route("api/command")]
        [HttpPost]
        // POST: /<controller>/
        public async Task<StatusCodeResult> Post([FromBody]Command newC)
        {
            cmd = new Command(newC.Aileron, newC.Rudder, newC.Elevator, newC.Throttle);
            var resultPost = await this.telnetClient.Execute(cmd);
            if (resultPost == Result.NotOk)
            {
                return StatusCode(500);
            }
            else
            {
                return StatusCode(200);
            }
        }

        [Route("screenshot")]
        [HttpGet]
        public IActionResult GetScreenshot()
        {
            ScreenController s = new ScreenController();
            byte[] data = s.GetImage();
            if (data == null)
            {
                return NotFound();
            }
            return File(data, "image/jpeg");
        }
    }
}
