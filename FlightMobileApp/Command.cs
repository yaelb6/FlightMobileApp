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

        [JsonPropertyName("elevator")]
        [JsonProperty("elevator")]
        public double Elevator { get; set; }

        [JsonPropertyName("throttle")]
        [JsonProperty("throttle")]
        public double Throttle { get; set; }

        [JsonConstructor]
        public Command(double aileron, double rudder, double elevator, double throttle)
        {
            Aileron = aileron;
            Rudder = rudder;
            Elevator = elevator;
            Throttle = throttle;
            
        }

        public Command()
        {
        }
    }
}
