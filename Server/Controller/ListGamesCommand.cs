using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.View;
using Newtonsoft.Json;

namespace Server.Controller
{
    /*
     * The list games command request the list of accessible multipley games
     * from the model, then returns them.
     */
    class ListGamesCommand : ICommand
    {
        private IModel model;

        public ListGamesCommand(IModel model)
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
            if (args.Count() != 0)
            {
                return Error.makeError("List does not take arguments");
            }
            string jsonParse = JsonConvert.SerializeObject(model.GetActiveGames());
            return new Result(jsonParse + '\n', false);
        }
    }
}
