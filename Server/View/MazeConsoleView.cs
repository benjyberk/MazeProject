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
    /*
     * The 'View' of the Maze Game handles the connection and communication with clients
     * attempting to play both single-player and multiplayer games.
     */
    public class MazeConsoleView : IView
    {
        private int port;
        private IPEndPoint ip;
        private TcpListener listener;
        private IController controller;
        private Task task;

        public MazeConsoleView(IController control)
        {
            IPAddress tempIP = IPAddress.Parse(ConfigurationManager.AppSettings["ip"]);
            port = int.Parse(ConfigurationManager.AppSettings["port"]);
            ip = new IPEndPoint(tempIP, port);
            controller = control;
        }

        /*
         * The start command begins the listening process in a new thread, this allows
         * for the generation and reception of multiple clients simultaneously.
         */
        public void start()
        {
            MazeClientHandler handler = new MazeClientHandler();
            listener = new TcpListener(ip);
            listener.Start();

            // We start the 'listening server' task
            task = new Task(() => {
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
                listener.Stop();
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

        public void wait()
        {
            task.Wait();
        }

        public void stop()
        {
            listener.Stop();
        }
    }
}
