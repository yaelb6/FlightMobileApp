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
            //Define Ip and port from app config.
            connectionIp = "127.0.0.1";
            connectionPort = 5403;
            firstConnection();
            Start();
            //tcpClient = new TcpClient(connectionIp, connectionPort);

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
            NetworkStream stream = tcpClient.GetStream();
            foreach (AsyncCommand command in commandsQueue.GetConsumingEnumerable())
            {
                string message = null;
                string response = null;
                Result res = Result.Ok;
                //get command 
                if (command.isGet == true)
                {
                    try
                    {
                        message = "get /controls/flight/aileron\n" + "get /controls/flight/rudder\n" +
                        "get /controls/flight/elevator\n" + "get /controls/engines/current-engine/throttle\n";
                        //response = GetAndSet(message, stream, true);
                        //Debug.WriteLine(response.ToString());
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                        tcpClient.GetStream().Write(data, 0, data.Length);
                        String responseData = String.Empty;
                        Byte[] readData = new byte[1024];
                        do
                        {
                            int bytes = tcpClient.GetStream().Read(readData, 0, 1024);
                            responseData = System.Text.Encoding.ASCII.GetString(readData, 0, bytes);
                        }
                        while (tcpClient.GetStream().DataAvailable);
                    }
                    catch(Exception)
                    {
                        res = Result.NotOk;
                    }
            }
                //set command
                else
                {
                    try
                    {
                        message = "set /controls/flight/aileron " + command.Command.Aileron.ToString() + "\n" +
                            "set /controls/flight/rudder " + command.Command.Rudder.ToString() + "\n" +
                        "set /controls/flight/elevator " + command.Command.Elevator.ToString() + "\n" +
                        "set /controls/engines/current-engine/throttle " + command.Command.Throttle.ToString() + "\n";
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                        // Send the message to the connected TcpServer - the server need to know what kind of data I want. 
                        tcpClient.GetStream().Write(data, 0, data.Length);
                        String responseData = String.Empty;
                    }
                    catch(Exception)
                    {
                        res = Result.NotOk;
                    }

                }
                if (response != null)
                {
                    command.Completion.SetResult(res);
                }
            }
        }
        public string GetAndSet(string message, NetworkStream stream, Boolean isGet)
        {
            // Receive the TcpServer.response.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            
            // Send the message to the connected TcpServer - the server need to know what kind of data I want. 
            tcpClient.GetStream().Write(data, 0, data.Length);
            String responseData = String.Empty;
            if (isGet == true)
            {
                //Read the first batch of the TcpServer response bytes.
                if (tcpClient.GetStream().CanRead)
                {
                    Byte[] readData = new byte[1024];
                    do
                    {
                        int bytes = tcpClient.GetStream().Read(readData, 0, 1024);
                        responseData = System.Text.Encoding.ASCII.GetString(readData, 0, bytes);
                    }
                    while (tcpClient.GetStream().DataAvailable);

                    //int bytes = stream.Read(readData, 0, 50);

                    return responseData;
                }
                else
                {
                    return "disconnected";
                }
            }
            else
            {
                return "ok";
            }
        }
        public void firstConnection()
        {
            tcpClient = new TcpClient(connectionIp, connectionPort);
            NetworkStream stream = tcpClient.GetStream();
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("data\n");
            // Send the message to the connected TcpServer - the server need to know what kind of data I want. 
            stream.Write(data, 0, data.Length);
        }
    }
}
