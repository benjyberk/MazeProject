using MazeLib;
using SearchAlgorithmsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IModel
    {
        SearchableMaze GenerateMaze(string name, int rows, int cols);
        Solution<Position> SolveMaze(string mazeName, int solutionType);
        string StartGame(string name, TcpClient user);
    }
}
