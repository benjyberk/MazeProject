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
     * The Start Game command initializes the game, and returns nothing (the user has to wait for
     * a second player to join the game)
     */
    class StartGameCommand : ICommand
    {
        IModel model;

        public StartGameCommand(IModel model)
        {
            this.model = model;
        }

        public Result Execute(string[] args, TcpClient client = null)
        {
            if (args.Count() != 3)
            {
                return Error.makeError("Not enough arguments provided");
            }

            int rows, cols;
            bool validRows = int.TryParse(args[1], out rows);
            bool validCols = int.TryParse(args[2], out cols);
            // If the two arguments provided are not integers, an error is returned
            if (!validRows || !validCols)
            {
                return Error.makeError("Invalid row/column number");
            }
            model.StartGame(args[0], rows, cols, client);
            return new Result(null, true);
        }
    }
}
