using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.View
{
    /// <summary>
    /// A client handler manages the details of communication of a single client instance
    /// with the provided controller
    /// </summary>
    public interface IClientHandler
    {
        void HandleClient(TcpClient client, IController controller);
    }
}
