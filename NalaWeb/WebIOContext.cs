using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using NathanWiles.Nala.IO;
using NathanWiles.Nala.Interpreting;
using NathanWiles.Nala.Errors;

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

            hubContext
                .Clients
                .All
                .SendAsync("OutputChange", Output);
        }

        public string ReadLine()
        {
            throw new Exception("The 'read' keyword is not supported in the web version of nala.");
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
