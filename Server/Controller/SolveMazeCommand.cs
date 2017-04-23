using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.View;
using SearchAlgorithmsLib;
using MazeLib;
using Newtonsoft.Json.Linq;

namespace Server.Controller
{
    /*
     * The 'Solve Maze' command calls to the model to generate a solution for the given maze
     * if the solution exists - then it gets converted into the correct format and returned.
     * Otherwise an error is returned
     */
    class SolveMazeCommand : ICommand
    {
        private IModel model;   

        public SolveMazeCommand(IModel model)
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
            if (args.Count() != 2)
            {
                return Error.makeError("Not enough arguments provided");
            }
            int type;
            bool valid = int.TryParse(args[1], out type);
            if (!valid || type > 1 || type < 0)
            {
                return Error.makeError("Invalid algorithm selected (only 0 and 1 are allowed)");
            }

            Solution<Position> solution = model.SolveMaze(args[0], type);
            // If the model returned null, the maze doesn't exist.
            if (solution == null)
            {
                return Error.makeError("Could not find a maze named " + args[0]);
            }
            else
            {
                // A helper method is used to translate the list of positions into 'directions'
                string directions = DirectionFromSolution(solution);
                JObject solutionObject = new JObject();
                solutionObject["Name"] = args[0];
                solutionObject["Solution"] = directions;
                solutionObject["NodesEvaluated"] = solution.nodesEvaluated;
                return new View.Result(solutionObject.ToString(), false);
            }

        }

        private string DirectionFromSolution(Solution<Position> solution)
        {
            Position previous = solution.solutionPath[0].state;
            string returnString = "";
            // We skip the first element (which will always exist - the minimum amount of moves
            // is 1
            foreach (State<Position> p in solution.solutionPath.Skip(1))
            {
                if (p.state.Col == previous.Col)
                {
                    if (p.state.Row > previous.Row)
                    {
                        // Down
                        returnString += "3";
                    }
                    else
                    {
                        // Up
                        returnString += "2";
                    }
                }
                else
                {
                    if (p.state.Col > previous.Col)
                    {
                        // Right
                        returnString += "1";
                    }
                    else
                    {
                        // Left
                        returnString += "0";
                    }
                }
                previous = p.state;
            }
            return returnString;
        }
    }
}
