using System.Net;

namespace MasterServer.Interfaces
{
    public interface IClientHandler
    {
        void HandleClient(HttpListenerContext tcpClient);
    }
}
