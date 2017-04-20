using Newtonsoft.Json;
using Server.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class MazeController : IController
    {
        private Dictionary<string, ICommand> commands;
        private IModel model;
        private IView view;

        public MazeController()
        {
            commands = new Dictionary<string, ICommand>();
        }

        public Result ExecuteCommand(string command, TcpClient client)
        {
            string[] comArr = command.Split(' ');
            string key = comArr[0];
            if (!commands.ContainsKey(key))
            {
                return Error.makeError("That command doesn't exist");
            }
            string[] args = comArr.Skip(1).ToArray();
            ICommand executable = commands[key];
            return executable.Execute(args, client);
        }

        public void setModel(IModel model)
        {
            this.model = model;
        }

        public void setView(IView view)
        {
            this.view = view;
        }
    }
}
