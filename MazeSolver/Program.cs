using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGeneratorLib;
using MazeLib;
using SearchAlgorithmsLib;

namespace MazeSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            BestFirstSearcher<Position> bfs = new BestFirstSearcher<Position>();
            DepthFirstSearcher<Position> dfs = new DepthFirstSearcher<Position>();
            DFSMazeGenerator mazeMaker = new DFSMazeGenerator();
            Maze m = mazeMaker.Generate(30, 30);
            SearchableMaze maze = new SearchableMaze(m);
            maze.print();

            if (bfs.search(maze) != null)
            {
                Console.WriteLine("Number of BFS evaluations: {0}", bfs.getNumberOfEvaluations());
            }
            else
            {
                Console.WriteLine(":(");
            }

            if (dfs.search(maze) != null)
            {
                Console.WriteLine("Number of DFS evaluations: {0}", dfs.getNumberOfEvaluations());
            }
            else
            {
                Console.WriteLine(":(");
            }

        }
    }
}
