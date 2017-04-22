using Server.View;
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
        Result ExecuteCommand(string command, TcpClient client);
        void SetModel(IModel model);
        void SetView(IView view);
    }
}
