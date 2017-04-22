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

        public CloseGameCommand(IModel model)
        {
            this.model = model;
        }

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
