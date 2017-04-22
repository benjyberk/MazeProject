using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.View
{
    /*
     * The MazeClient Handler deals with the operations of a single client
     * Sending on messages to the controller until receiving the signal to close
     * the socket
     */
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
                        // If there is a message to be sent back to the client after the
                        // command, then send it.
                        if (result.resultString != null)
                        {
                            writer.Write(result.resultString);
                            writer.Flush();
                        }
                    }
                    // This exception catcher mostly happens when the client exits from
                    // The server unexpextedly
                    catch (Exception e)
                    {
                        result = null;
                        break;
                    }
                    finally
                    {
                        // The second part of the if is only reached if the first part doesn't occur
                        if (result == null || !result.keepOpen)
                        {
                            Console.WriteLine("Closing Socket");
                            writer.Dispose();
                            reader.Dispose();
                            stream.Dispose();
                            client.Close();
                        }
                    }
                } while (result != null && result.keepOpen);

            }).Start();
        }
    }
}
