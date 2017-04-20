using Newtonsoft.Json;
using Server.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller
{
    /*
     * This command attempts to generate a maze and return it to the user, if it fails it returns
     * a JSON formatted error, if it succeeds, it returns a JSON formmated maze.
     */
    class GenerateMazeCommand : ICommand
    {
        IModel model;

        public GenerateMazeCommand(IModel model)
        {
            this.model = model;
        }

        public Result Execute(string[] args, TcpClient client = null)
        {
            SearchableMaze maze;
            string returnLine;
            if (args.Count() == 3)
            {
                string mazeName = args[0];
                int rows = 0, cols = 0;
                try
                {
                    rows = int.Parse(args[1]);
                    cols = int.Parse(args[2]);
                    maze = model.GenerateMaze(mazeName, rows, cols);
                    returnLine = maze.toJSON();
                }
                catch
                {
                    return Error.makeError("Invalid row/column parameters");
                }
            }
            else
            {
                return Error.makeError("Not enough parameters");
            }

            return new Result(returnLine, false);
        }
    }
}
