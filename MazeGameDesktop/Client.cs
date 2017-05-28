using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGameDesktop
{
    /*
     * The client basically consists of two running tasks, a messager and a listener.
     * The messager constantly requests user input to send to the server; the listener
     * creates the connection, listens for responses (and outputs them), and then
     * keeps track of when the server closes the connection
     */
    class Client : INotifyPropertyChanged
    {
        private bool running;
        private int port;
        private IPEndPoint ip;
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        public bool ReopenFlag;

        /// <summary>
        /// The constructor reads from app.config the required settings
        /// </summary>
        public Client()
        {
            ReopenFlag = false;
            running = false;
            SetPortAndIP();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetPortAndIP()
        {
            IPAddress tempIP = IPAddress.Parse(Properties.Settings.Default.IP);
            port = int.Parse(Properties.Settings.Default.Port);
            ip = new IPEndPoint(tempIP, port);
        }

        /// <summary>
        /// Stops the currently executing client if it is executing
        /// </summary>
        public void stop()
        {
            ReopenFlag = false;
            if (running)
            {
                running = false;
                try
                {
                    client.Close();
                }
                catch
                {
                    // Error closing
                }
            }
        }

        /// <summary>
        /// Sends data to the connected server
        /// </summary>
        /// <param name="data">Information to send</param>
        public void sendData(string data)
        {
            if (running)
            {
                writer.WriteLine(data);
                writer.Flush();
            }
        }

        public bool IsRunning()
        {
            return running;
        }

        /// <summary>
        /// The start command begins the two tasks that make up the client
        /// </summary>
        public void start()
        {
            SetPortAndIP();
            // We start the 'listening server' task and start it
            // It stops when the server closes the connection
            Task listenTask = new Task(() =>
            {
                do
                {
                    try
                    {
                        client = new TcpClient();
                        client.Connect(ip);
                    }
                    catch
                    {
                        Debug.WriteLine("Error at socket connect");
                        ReopenFlag = false;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Error.makeError("Error at socket connect")));
                        return;
                    }
                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);

                    string result = null;
                    running = true;
                    do
                    {
                        try
                        {
                            Debug.WriteLine("Client Waiting For Response");
                            result = reader.ReadLine();
                            // When readline returns null, the server has closed the socket
                            if (result != null)
                            {
                                string finalResult = null;
                                if (result.EndsWith("XXX"))
                                {
                                    finalResult = result.TrimEnd('X');
                                    result = null;
                                } else
                                {
                                    finalResult = result;
                                }
                                Debug.WriteLine("Client got result {0}", finalResult, "");
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(finalResult));
                                
                            }
                        }
                        catch
                        {
                            break;
                        }
                    } while (result != null && running);
                    // We allow the option of automatically reconnecting
                } while (ReopenFlag);
                Debug.WriteLine("Client Closing");
                running = false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Error.makeError("Socket Closed")));
                reader.Dispose();
                writer.Dispose();
                stream.Dispose();
                client.Close();
            });
            // We start the listening task
            listenTask.Start();
        }
    }
}
