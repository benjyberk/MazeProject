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
    class Client
    {
        private int port;
        private IPEndPoint ip;
        private TcpListener listener;
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
            // We start the 'listening server' task
            Task listenTask = new Task(() => {
                while (true)
                {
                    client.Connect(ip);
                    Console.WriteLine("New connection to server - listening");
                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);
                    string result = null;
                    do
                    {
                        result = reader.ReadLine();
                        if (result != null)
                        {
                            Console.WriteLine(result);
                        }
                    } while (result != null);
                    reader.Dispose();
                    writer.Dispose();
                    stream.Dispose();
                    client.Close();

                    client = new TcpClient();
                    Console.WriteLine("Socket closed");
                }
            });
            Task messageTask = new Task(() => {
                    while (true)
                    {
                        Console.Write("Please enter a command: ");
                        string command = Console.ReadLine();
                        Console.WriteLine("You entered the command: {0}", command);

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
