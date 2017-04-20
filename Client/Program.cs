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
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress tempIP = IPAddress.Parse(ConfigurationManager.AppSettings["ip"]);
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            IPEndPoint ip = new IPEndPoint(tempIP, port);
            TcpClient client = new TcpClient();

            NetworkStream stream = null;
            BinaryReader reader = null;
            BinaryWriter writer = null;

            while (true)
            {
                if (!client.Connected)
                {
                    client.Connect(ip);
                    Console.WriteLine("You are connected");
                    stream = client.GetStream();
                    reader = new BinaryReader(stream);
                    writer = new BinaryWriter(stream);
                }
                // Send data to server
                Console.Write("Please enter a command: ");
                string command = Console.ReadLine();
                writer.Write(command);
                writer.Flush();
                // Get result from server
                string result = reader.ReadString();
                Console.WriteLine("Result = {0}", result);
            }
        }
    }
}
