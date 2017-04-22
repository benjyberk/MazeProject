using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.View;

namespace Server.Controller
{
    class PlayCommand : ICommand
    {
        IModel model;
        public PlayCommand(IModel model)
        {
            this.model = model;
        }

        public Result Execute(string[] args, TcpClient client = null)
        {
            string[] valid = { "up", "down", "left", "right" };
            // For these specific errors, we allow the player to keep the connection open
            // So we change the default behaviour of error messages [they dont close the socket]
            Result r;
            if (args.Count() != 1)
            {
                r = Error.makeError("Insufficient arguments provided: only 1 is needed");
                r.keepOpen = true;
                return r;
            }
            if (!valid.Contains(args[0]))
            {
                r = Error.makeError("Only 'up', 'down', 'left' and 'right' are valid moves");
                r.keepOpen = true;
                return r;
            }

            // We now contact the model and attempt to make the move (this fails if the
            // client hasn't yet started/joined a game)
            bool success = model.MakeMove(client, args[0]);
            if (!success)
            {
                r = Error.makeError("You are not connected to any games!");
                r.keepOpen = true;
                return r;
            }
            // If we get here, the move was performed successfully
            return new Result(null, true);
        }
    }
}
