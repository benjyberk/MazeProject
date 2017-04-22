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
        /*
         * The MainLine creates the MVC [with a controller connected to a view and model]
         * and begins the 'view'.
         */
        static void Main(string[] args)
        {
            IModel model = new MazeGameModel();
            IController controller = new MazeController(model);
            MazeConsoleView view = new MazeConsoleView(controller);
            view.start();
            view.wait();
        }
    }
}
