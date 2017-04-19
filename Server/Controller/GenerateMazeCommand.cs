using Newtonsoft.Json;
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

        public string Execute(string[] args, TcpClient client = null)
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
                    returnLine = JsonConvert.SerializeObject(new Error("Invalid row/column parameters"));
                }
            }
            else
            {
                returnLine = JsonConvert.SerializeObject(new Error("Not enough parameters"));
            }

            return returnLine;
        }
    }
}
