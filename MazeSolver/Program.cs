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
            ISearcher<Position> bfs = new BestFirstSearcher<Position>();
            ISearcher<Position> dfs = new DepthFirstSearcher<Position>();
            DFSMazeGenerator mazeMaker = new DFSMazeGenerator();
            MazeWrap m = new MazeWrap();
            Console.WriteLine("Start {0} - End {1}", m.InitialPos.ToString(), m.GoalPos.ToString());
            Console.WriteLine("Rows {0} - Cols {1}", m.Rows, m.Cols);
            SearchableMaze maze = new SearchableMaze(m);
            Maze k;
           
            maze.print();
            if (bfs.search(maze) != null)
            {
                Console.WriteLine("Number of BFS evaluations: {0}", bfs.getNumberOfEvaluations());
            }
            else
            {
                Console.WriteLine("No solution found!");
            }

            if (dfs.search(maze) != null)
            {
                Console.WriteLine("Number of DFS evaluations: {0}", dfs.getNumberOfEvaluations());
            }
            else
            {
                Console.WriteLine("No solution found!");
            }

        }
    }
}
