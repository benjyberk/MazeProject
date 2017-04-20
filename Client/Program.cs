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
            Client client = new Client();
            client.start();
            Console.ReadKey();
        }

        private static Task clientMessageTask(ref StreamWriter writer)
        {
            StreamWriter s = writer;
            Task t = new Task(() =>
            {
                while (true)
                {
                    Console.Write("Please enter a command: ");
                    string command = Console.ReadLine();
                    Console.WriteLine("You entered the command: {0}", command);

                    s.WriteLine(command);
                    s.Flush();
                }
            });

            return t;
        }

        private static Task clientListenTask(StreamReader reader)
        {
            Task t = new Task(() =>
            {
                while (true)
                {
                    string result = reader.ReadLine();
                    while (result != null)
                    {
                        // Get result from server
                        result = reader.ReadLine();

                        Console.WriteLine("Result = {0}", result);

                    }
                }
            });

            return t;
        }
    }
}

