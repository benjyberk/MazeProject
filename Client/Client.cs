using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /*
     * The client basically consists of two running tasks, a messager and a listener.
     * The messager constantly requests user input to send to the server; the listener
     * creates the connection, listens for responses (and outputs them), and then
     * keeps track of when the server closes the connection
     */
    class Client
    {
        private int port;
        private IPEndPoint ip;
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;


        public Client()
        {
            IPAddress tempIP = IPAddress.Parse(ConfigurationManager.AppSettings["ip"]);
            port = int.Parse(ConfigurationManager.AppSettings["port"]);
            ip = new IPEndPoint(tempIP, port);
            client = new TcpClient();
        }

        public void start()
        {
            bool done = false;
            // We start the 'listening server' task
            Task listenTask = new Task(() => {
                while (!done)
                {
                    client.Connect(ip);
                    Console.WriteLine("New Connection to Server:");
                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);
                    string result = null;
                    do
                    {
                        try
                        {
                            result = reader.ReadLine();
                            // When readline returns null, the server has closed the socket
                            if (result != null)
                            {
                                Console.WriteLine(result);
                            }
                        }
                        catch
                        {
                            done = true;
                            break;
                        }
                    } while (result != null);
                    reader.Dispose();
                    writer.Dispose();
                    stream.Dispose();
                    client.Close();

                    client = new TcpClient();
                }
            });
            // We start the messaging task
            Task messageTask = new Task(() => {
                    while (true)
                    {
                        string command = Console.ReadLine();

                        writer.WriteLine(command);
                        writer.Flush();
                    }
            });
            listenTask.Start();
            messageTask.Start();
            listenTask.Wait();
        }
    }
}
