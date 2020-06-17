using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp
{
    public enum Result { Ok, NotOk }
    public class AsyncCommand
    {
        public Command Command { get; private set; }
        public Task<Result> Task { get => Completion.Task; }
        public TaskCompletionSource<Result> Completion { get; private set; }
        public Boolean isGet { get; set; }
        public AsyncCommand(Command input)
        {
            Command = input;
            Completion = new TaskCompletionSource<Result>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }
}
