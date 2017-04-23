using Newtonsoft.Json;
using Server.Controller;
using Server.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /*
     * The Controller handles reception of commands from the view, then identifies
     * the given command - if it exists as a valid command, the appropriate
     * ICommand is run.
     */
    class MazeController : IController
    {
        private Dictionary<string, ICommand> commands;
        private IModel model;
        private IView view;

        /// <summary>
        /// The constructor requires the model to be provided, and adds all possible commands
        /// to the dictionary of commands
        /// </summary>
        /// <param name="model">The model used for communication</param>
        public MazeController(IModel model)
        {
            // All needed commands are added on construction.
            commands = new Dictionary<string, ICommand>();
            commands.Add("generate", new GenerateMazeCommand(model));
            commands.Add("solve", new SolveMazeCommand(model));
            commands.Add("start", new StartGameCommand(model));
            commands.Add("list", new ListGamesCommand(model));
            commands.Add("join", new JoinGameCommand(model));
            commands.Add("play", new PlayCommand(model));
            commands.Add("close", new CloseGameCommand(model));
        }

        /// <summary>
        /// Executes a given command by associating it with one of the contained ICommands
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="client">The client provided</param>
        /// <returns>The result of the command</returns>
        public Result ExecuteCommand(string command, TcpClient client)
        {
            // The command is broken down into 'command' and 'arguments'
            string[] comArr = command.Split(' ');
            string key = comArr[0];
            if (!commands.ContainsKey(key))
            {
                return Error.makeError("That command doesn't exist");
            }
            // The first 'command' is skipped - as it is not an argument.
            string[] args = comArr.Skip(1).ToArray();
            ICommand executable = commands[key];
            // The command is run, and its result returned
            return executable.Execute(args, client);
        }

        public void SetModel(IModel model)
        {
            this.model = model;
        }

        public void SetView(IView view)
        {
            this.view = view;
        }
    }
}
