using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.View;

namespace Server.Controller
{
    /*
     * The Join Game command attempts to join an already existing multiplayer game
     */
    class JoinGameCommand : ICommand
    {
        private IModel model;

        public JoinGameCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Executes the given command
        /// </summary>
        /// <param name="args">The arguments for the command</param>
        /// <param name="client">The client sending the request</param>
        /// <returns>The result of the command</returns>
        public Result Execute(string[] args, TcpClient client = null)
        {
            // If insufficient arguments are provided (or too many), the server rejects
            if (args.Count() != 1)
            {
                return Error.makeError("Incorrect arguments provided");
            }
            // We attempt to join the multiplayer game, if we succeed the client receives nothing
            bool success = model.JoinMultiplayerGame(args[0], client);
            if (success)
            {
                return new View.Result(null, true);
            }
            // If we fail, we receive an error
            else
            {
                return Error.makeError("Unable to join game, either it doesn't exist or is full");
            }
        }
    }
}
