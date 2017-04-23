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
    /// <summary>
    /// The mainline of the client practically only runs the client
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main class
        /// </summary>
        /// <param name="args">Commandline arguments</param>
        static void Main(string[] args)
        {
            Client client = new Client();
            client.start();
        }
    }
}

