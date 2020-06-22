//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Routing;
//using Newtonsoft.Json;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace FlightMobileApp
//{
//    public class HomeController : Controller
//    {
//        SimulatorClient telnetClient;
//        public Command cmd { get; set; }

//        // GET: /<controller>/
//        public async Task<ActionResult<Command>> Get()
//        {
//            Boolean isGet = true;
//            var contentt = await this.telnetClient.Execute(cmd, isGet);
//            cmd = JsonConvert.DeserializeObject<Command>(cmd.ToString());
//            return cmd;
//        }


       
//        [Route("api/command")]
//        // POST: /<controller>/
//        public async Task<ActionResult<Command>> Post(Command newC)
//        {
//            Debug.WriteLine("IN POST");
//            Console.WriteLine("IN POST");
//            Boolean isGet = false;
//            //cmd = newC;
//            var contentt = await this.telnetClient.Execute(newC, isGet);
//            //cmd = JsonConvert.DeserializeObject<Command>(cmd.ToString());
//            Debug.WriteLine("afer EXECUTE");
//            Console.WriteLine("afer EXECUTE");
//            return newC;
//        }
//    }
//}
