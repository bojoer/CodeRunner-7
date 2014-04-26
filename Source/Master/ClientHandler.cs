using System.Net.Sockets;
using MasterServer.Interfaces;

namespace MasterServer
{
    class ClientHandler : IClientHandler
    {
        public void HandleClient(TcpClient tcpClient)
        {
            var networkStream = tcpClient.GetStream();

//            ReadFileFromClient(networkStream);

            ICodeRunner codeRunner = new CodeRunner();

//            var result = codeRunner.Run();

//            ReturnResultToClient(result);
        }
    }
}