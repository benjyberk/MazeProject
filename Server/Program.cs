using Server.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IController controller = new MazeController();
            IModel model = new MazeGameModel();
            IView view = new MazeConsoleView(controller);
            view.start();
            Console.ReadKey();
        }
    }
}
