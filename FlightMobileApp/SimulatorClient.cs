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
        public SimulatorClient()
        {
            commandsQueue = new BlockingCollection<AsyncCommand>();
            tcpClient = new TcpClient();
        }
        // Called by the WebApi Controller, it will await on the returned Task<>
        // This is not an async method, since it does not await anything.
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
            tcpClient.Connect("127.0.0.1", 5402);
            NetworkStream stream = tcpClient.GetStream();
            foreach (AsyncCommand command in commandsQueue.GetConsumingEnumerable())
            {
                byte[] sendBuffer = "set ";
                byte[] recvBuffer = new byte[1024];
                stream.Write(sendBuffer, 0, sendBuffer.Length);
                int nRead = stream.Read(recvBuffer, 0, 1024);
                Result res = // recvBuffer to Result
                             // TaskCompletionSource allows an external thread to set
                             // the result (or the exceptino) on the associated task object
                command.Completion.SetResult(res);
            }
        }
    }
}
