using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Server.View
{
    class MazeConsoleView : IView
    {
        private int port;
        private IPEndPoint ip;
        private TcpListener listener;
        private IController controller;

        public MazeConsoleView(IController control)
        {
            IPAddress tempIP = IPAddress.Parse(ConfigurationManager.AppSettings["ip"]);
            port = int.Parse(ConfigurationManager.AppSettings["port"]);
            ip = new IPEndPoint(tempIP, port);
            controller = control;
        }

        public void sendInfo(string data, TcpClient user)
        {
            throw new NotImplementedException();
        }

        public void start()
        {
            MazeClientHandler handler = new MazeClientHandler();
            listener = new TcpListener(ip);
            listener.Start();

            // We start the 'listening server' task
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        handler.HandleClient(client, controller);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

        public void stop()
        {
            listener.Stop();
        }
    }
}
