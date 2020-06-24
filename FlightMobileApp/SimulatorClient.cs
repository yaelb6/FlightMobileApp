using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightMobileApp
{
    public class SimulatorClient
    {
        private readonly BlockingCollection<AsyncCommand> commandsQueue;
        private TcpClient tcpClient;
        string connectionIp;
        int connectionPort;
        public SimulatorClient()
        {
            commandsQueue = new BlockingCollection<AsyncCommand>();
            connectionIp = "127.0.0.1";
            connectionPort = 5403;
            tcpClient = new TcpClient();
            Start();
        }
        public Task<Result> Execute(Command cmd)
        {
            var asyncCommand = new AsyncCommand(cmd);
            commandsQueue.Add(asyncCommand);
            return asyncCommand.Task;
        }
        public void Start()
        {
            Task.Factory.StartNew(ProcessCommands);
        }

        public void ProcessCommands()
        {
            tcpClient.Connect(connectionIp, connectionPort);
            NetworkStream stream = tcpClient.GetStream();
            firstConnection();
            foreach (AsyncCommand command in commandsQueue.GetConsumingEnumerable())
            {
                string message = null;
                string response = null;
                Result res = Result.Ok;
                message = "set /controls/flight/aileron " + command.Command.Aileron.ToString() + "\n" +
                "set /controls/flight/rudder " + command.Command.Rudder.ToString() + "\n" +
                "set /controls/flight/elevator " + command.Command.Elevator.ToString() + "\n" +
                "set /controls/engines/current-engine/throttle " + command.Command.Throttle.ToString() + "\n";
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Send the message to the connected TcpServer - the server need to know what kind of data I want. 
                tcpClient.GetStream().Write(data, 0, data.Length);
                if (GetAndCheck(command.Command) == false)
                {
                    res = Result.NotOk;
                }
                command.Completion.SetResult(res);
            }
        }
        public bool GetAndCheck(Command cmd)
        {
            string message;
            message = "get /controls/flight/aileron\n";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            tcpClient.GetStream().Write(data, 0, data.Length);
            String responseData = String.Empty;
            Byte[] readData = new byte[1024];
            if (tcpClient.GetStream().CanRead)
            {
                int bytes = tcpClient.GetStream().Read(readData, 0, 1024);
                responseData = System.Text.Encoding.ASCII.GetString(readData, 0, bytes);
                Debug.WriteLine("aileron: ", responseData);
                if (Math.Abs(cmd.Aileron - double.Parse(responseData)) > 0.00001)
                {
                    return false;
                }
            }
            message = "get /controls/flight/rudder\n";
            data = System.Text.Encoding.ASCII.GetBytes(message);
            tcpClient.GetStream().Write(data, 0, data.Length);
            responseData = String.Empty;
            readData = new byte[1024];
            if (tcpClient.GetStream().CanRead)
            {
                int bytes = tcpClient.GetStream().Read(readData, 0, 1024);
                responseData = System.Text.Encoding.ASCII.GetString(readData, 0, bytes);
                Debug.WriteLine("rudder: ", responseData);
                if (Math.Abs(cmd.Rudder - double.Parse(responseData)) > 0.00001)
                {
                    return false;
                }
            }
            message = "get /controls/flight/elevator\n";
            data = System.Text.Encoding.ASCII.GetBytes(message);
            tcpClient.GetStream().Write(data, 0, data.Length);
            responseData = String.Empty;
            readData = new byte[1024];
            if (tcpClient.GetStream().CanRead)
            {
                int bytes = tcpClient.GetStream().Read(readData, 0, 1024);
                responseData = System.Text.Encoding.ASCII.GetString(readData, 0, bytes);
                Debug.WriteLine("elevator: ", responseData);
                if (Math.Abs(cmd.Elevator - double.Parse(responseData)) > 0.00001)
                {
                    return false;
                }
            }
            message = "get /controls/engines/current-engine/throttle\n";
            data = System.Text.Encoding.ASCII.GetBytes(message);
            tcpClient.GetStream().Write(data, 0, data.Length);
            responseData = String.Empty;
            readData = new byte[1024];
            if (tcpClient.GetStream().CanRead)
            {
                int bytes = tcpClient.GetStream().Read(readData, 0, 1024);
                responseData = System.Text.Encoding.ASCII.GetString(readData, 0, bytes);
                Debug.WriteLine("throttle: ", responseData);
                if (Math.Abs(cmd.Throttle - double.Parse(responseData)) > 0.00001)
                {
                    return false;
                }
            }
            return true;
        }
        public void firstConnection()
        {
            NetworkStream stream = tcpClient.GetStream();
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("data\n"); 
            stream.Write(data, 0, data.Length);
        }
    }
}
