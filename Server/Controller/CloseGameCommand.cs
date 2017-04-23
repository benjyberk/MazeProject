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
     * The cancel game command simply attempts to cancel the game - and returns normally
     * if the client who attempts to cancel the game is part of the game
     */
    class CloseGameCommand : ICommand
    {
        private IModel model;
        /// <summary>
        /// The constructor for the command
        /// </summary>
        /// <param name="model">The model - used to communicate commands</param>
        public CloseGameCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Executes the Close Game command
        /// </summary>
        /// <param name="args">The arguments for the command (none in this case)</param>
        /// <param name="client">The client sending the request</param>
        /// <returns></returns>
        public Result Execute(string[] args, TcpClient client = null)
        {
            bool success = model.CancelGame(client);
            if (!success)
            {
                return Error.makeError("You cannot cancel a game because you haven't joined any games.");
            }
            return new View.Result(null, false);
        }
    }
}
