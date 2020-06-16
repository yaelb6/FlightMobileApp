using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FlightMobileApp
{
    public class Command
    {
        [JsonPropertyName("aileron")]
        [JsonProperty("aileron")]
        public double Aileron { get; set; }

        [JsonPropertyName("rudder")]
        [JsonProperty("rudder")]
        public double Rudder { get; set; }

        [JsonPropertyName("throttle")]
        [JsonProperty("throttle")]
        public double Throttle { get; set; }

        [JsonPropertyName("elevator")]
        [JsonProperty("elevator")]
        public double Elevator { get; set; }

        public Command(double Aileron1, double Rudder1, double Throttle1, double Elevator1)
        {
            Aileron = Aileron1;
            Rudder = Rudder1;
            Throttle = Throttle1;
            Elevator = Elevator1;
        }
    }
}
