using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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

        /// <summary>
        /// The constructor reads from app.config the required settings
        /// </summary>
        public Client()
        {
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

        /// <summary>
        /// The start command begins the two tasks that make up the client
        /// </summary>
        public void start()
        {
            SetPortAndIP();
            running = true;
            // We start the 'listening server' task and start it
            // It stops when the server closes the connection
            Task listenTask = new Task(() =>
            {
                client = new TcpClient();
                client.Connect(ip);
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
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(result));
                        }
                    }
                    catch
                    {
                        running = false;
                        break;
                    }
                } while (result != null && running);

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
