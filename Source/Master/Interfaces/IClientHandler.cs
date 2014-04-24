using System.Net.Sockets;

namespace MasterServer.Interfaces
{
    public interface IClientHandler
    {
        void HandleClient(TcpClient tcpClient);
    }
}
