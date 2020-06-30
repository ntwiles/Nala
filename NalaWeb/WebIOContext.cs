using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using NathanWiles.Nala.IO;

using NalaWeb.Hubs;


namespace NalaWeb
{
    public class WebIOContext : IIOContext
    {
        private bool waitingForInput;
        public List<string> Output { get; }
        private string Input;

        private readonly IHubContext<NalaHub> hubContext;

        public WebIOContext(IHubContext<NalaHub> hubContext)
        {
            Output = new List<string>();
            this.hubContext = hubContext;
        }

        public void Clear()
        {
            Output.Clear();
        }

        public string ReadLine()
        {
            waitingForInput = true;

            while (waitingForInput) { }

            return Input;
        }

        public void Write(string message)
        {
            Output.Add(message);

            hubContext
                .Clients
                .All
                .SendAsync("OutputChange", Output);
        }

        public void WriteLine(string message)
        {
            Output.Add(message + "<br>");

            hubContext
                .Clients
                .All
                .SendAsync("OutputChange", Output);
        }

        public void SendInput(string input)
        {
            Input = input;
            waitingForInput = false;
        }
    }
}
