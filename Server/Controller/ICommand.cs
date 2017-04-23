using Server.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface ICommand
    {
        /// <summary>
        /// Executes the given command
        /// </summary>
        /// <param name="args">The arguments to the command</param>
        /// <param name="client">The client performing the request</param>
        /// <returns></returns>
        Result Execute(string[] args, TcpClient client = null);
    }
}
