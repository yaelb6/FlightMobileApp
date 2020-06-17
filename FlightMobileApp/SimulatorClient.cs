using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightMobileApp
{
    public class SimulatorClient
    {
        private readonly BlockingCollection<AsyncCommand> commandsQueue;
        private readonly TcpClient tcpClient;
        string connectionIp;
        int connectionPort;
        public SimulatorClient()
        {
            commandsQueue = new BlockingCollection<AsyncCommand>();
            tcpClient = new TcpClient();
            //Define Ip and port from app config.
            connectionIp = "127.0.0.1";
            connectionPort = 5402;
        }
        // Called by the WebApi Controller, it will await on the returned Task<>
        // This is not an async method, since it does not await anything.
        public Task<Result> Execute(Command cmd, Boolean getOrSet)
        {
            var asyncCommand = new AsyncCommand(cmd);
            asyncCommand.isGet = getOrSet;
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
            foreach (AsyncCommand command in commandsQueue.GetConsumingEnumerable())
            {
                string message, response;
                Result res = Result.Ok;
                //get command 
                if (command.isGet == true)
                {
                    message = "get /controls/flight/rudder\n";
                    response = GetAndSet(message, stream);
                    if (!response.Equals("disconnected"))
                    {
                        command.Command.Rudder = Double.Parse(response);
                    }
                    else
                    {
                        res = Result.NotOk;
                    }
                    message = "get /controls/flight/throttle\n";
                    response = GetAndSet(message, stream);
                    if (!response.Equals("disconnected"))
                    {
                        command.Command.Throttle = Double.Parse(response);
                    }
                    else
                    {
                        res = Result.NotOk;
                    }
                    message = "get /controls/flight/elevator\n";
                    response = GetAndSet(message, stream);
                    if (!response.Equals("disconnected"))
                    {
                        command.Command.Elevator = Double.Parse(response);
                    }
                    else
                    {
                        res = Result.NotOk;
                    }
                    message = "get /controls/flight/aileron\n";
                    response = GetAndSet(message, stream);
                    if (!response.Equals("disconnected"))
                    {
                        command.Command.Aileron = Double.Parse(response);
                    }
                    else
                    {
                        res = Result.NotOk;
                    }
                }
                //set command
                else
                {
                    message = "set /controls/flight/rudder " + command.Command.Rudder + "\n";
                    response = GetAndSet(message, stream);
                    //send error to client
                    if (response.Equals("disconnected"))
                    {
                        res = Result.NotOk;
                    }
                    message = "set /controls/flight/throttle " + command.Command.Throttle + "\n";
                    response = GetAndSet(message, stream);
                    //send error to client
                    if (response.Equals("disconnected"))
                    {
                        res = Result.NotOk;
                    }
                    message = "set /controls/flight/elevator " + command.Command.Elevator + "\n";
                    response = GetAndSet(message, stream);
                    //send error to client
                    if (response.Equals("disconnected"))
                    {
                        res = Result.NotOk;
                    }
                    message = "set /controls/flight/aileron " + command.Command.Aileron + "\n";
                    response = GetAndSet(message, stream);
                    //send error to client
                    if (response.Equals("disconnected"))
                    {
                        res = Result.NotOk;
                    }
                }
                command.Completion.SetResult(res);
            }
        }
        public string GetAndSet(string message, NetworkStream stream)
        {
            // Receive the TcpServer.response.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            try
            {
                stream = tcpClient.GetStream();
            }
            catch (InvalidOperationException)
            {
                return "disconnected";
            }
            // Send the message to the connected TcpServer - the server need to know what kind of data I want. 
            stream.Write(data, 0, data.Length);
            String responseData = String.Empty;
            // Read the first batch of the TcpServer response bytes.
            if (stream.CanRead)
            {
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                return responseData;
            }
            else
            {
                return "disconnected";
            }
        }
    }
}
