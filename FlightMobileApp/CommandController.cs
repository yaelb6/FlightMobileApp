using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;
using Microsoft.Extensions.Hosting.Internal;
using static System.Net.Mime.MediaTypeNames;
//using RouteAttribute = System.Web.Http.RouteAttribute;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightMobileApp
{
    public class CommandController : Controller
    {
        private SimulatorClient telnetClient;
        private Boolean firstTime = false;
        public Command cmd { get; set; }

        public CommandController()
        {
            this.telnetClient = new SimulatorClient();
            telnetClient.Start();
        }

        //GET: /<controller>/
        [Route("api/command")]
        [HttpGet]
        public async Task<ActionResult<Command>> Get()
        {
            Boolean isGet = true;
            var contentt = await this.telnetClient.Execute(cmd, isGet);
            cmd = JsonConvert.DeserializeObject<Command>(cmd.ToString());
            return cmd;
        }

        [Route("api/command")]
        [HttpPost]
        // POST: /<controller>/
        public async Task<ActionResult<Command>> Post([FromBody]Command newC)
        {
            Debug.WriteLine("IN POST");
            Console.WriteLine("IN POST");

            Boolean isGet = false;
            Debug.WriteLine("Elevator = ", newC.Elevator.ToString());
            Console.WriteLine("Elevator = ", newC.Elevator.ToString());

            cmd = new Command(newC.Aileron, newC.Rudder, newC.Elevator, newC.Throttle);
            var contentt = await this.telnetClient.Execute(cmd, isGet);

            Debug.WriteLine("AFTERRRRRR");
            Console.WriteLine("AFTERRRRRR");
            return cmd;
        }

        [Route("screenshot")]
        [HttpGet]
        public IActionResult GetScreenshot()
        {
            if (firstTime == false)
            {
                firstTime = true;
                telnetClient.firstConnection();
            }
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
