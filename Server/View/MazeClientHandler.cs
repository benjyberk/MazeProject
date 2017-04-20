using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.View
{
    class MazeClientHandler : IClientHandler
    {
        public void HandleClient(TcpClient client, IController control)
        {
            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);
                Result result = null;
                do
                {
                    try
                    {
                        string commandLine = reader.ReadLine();
                        Console.WriteLine("Got command: {0}", commandLine);
                        result = control.ExecuteCommand(commandLine, client);
                        writer.Write(result.resultString);
                        writer.Flush();
                    }
                    finally
                    {
                        if (!result.keepOpen)
                        {
                            writer.Dispose();
                            reader.Dispose();
                            stream.Dispose();
                            client.Close();
                        }
                    }
                } while (result.keepOpen);

            }).Start();
        }
    }
}
