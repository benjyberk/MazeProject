using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IController
    {
        string ExecuteCommand(string command, TcpClient client);
        void setModel(IModel model);
        void setView(IView view);
    }
}
