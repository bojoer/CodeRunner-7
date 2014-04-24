using System.Net;
using System.Net.Sockets;
using System.Threading;
using MasterServer.Interfaces;

namespace MasterServer
{
    class Master: IMaster
    {
        private TcpListener TcpListener { set; get; }
        private const int ListeningPort = 8000;
        private const string ListeningIpAddress = "127.0.0.1";

        public Master()
        {
            TcpListener = new TcpListener(IPAddress.Parse(ListeningIpAddress), ListeningPort);
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
