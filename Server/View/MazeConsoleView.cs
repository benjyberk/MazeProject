using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.View
{
    class MazeConsoleView : IView
    {
        private int port;
        private IPAddress ip;
        private TcpListener listener;
        private IController controller;

        public MazeConsoleView()
        {

        }

        public void sendInfo(string data, TcpClient user)
        {
            throw new NotImplementedException();
        }

        public void start()
        {
            throw new NotImplementedException();
        }
    }
}
