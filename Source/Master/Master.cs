using System.Net;
using System.Net.Sockets;
using System.Threading;
using MasterServer.Constants;
using MasterServer.Interfaces;

namespace MasterServer
{
    class Master: IMaster
    {
        private TcpListener TcpListener { set; get; }

        public Master()
        {
            TcpListener = new TcpListener(IPAddress.Parse(EnvironmentConstants.ServerIp), EnvironmentConstants.ServerPort);
        }

        public void Start()
        {
            ListenForConnections();
        }

        private void ListenForConnections()
        {
            while (true)
            {
                var tcpClient = TcpListener.AcceptTcpClient();
                var workerThread = new Thread(HandleClient);
                workerThread.Start(tcpClient);
            }
        }

        private void HandleClient(object client)
        {
            var tcpClient = (TcpClient) client;
        }
    }
}
