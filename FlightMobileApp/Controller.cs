using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp
{
    public class Controller
    {
        SimulatorClient telnetClient;
        private Command c;
        public Controller(SimulatorClient telnetClient)
        {
            this.telnetClient = telnetClient;
            //stop = false;
            //test
        }

        public void Connect()
        {
            telnetClient.Connect();
        }
        public void Disconnect()
        {
            //stop = true;
            telnetClient.Disconnect();
        }



    }
}
