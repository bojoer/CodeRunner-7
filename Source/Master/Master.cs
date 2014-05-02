using System.Net;
using System.Threading;
using MasterServer.Constants;
using MasterServer.Interfaces;

namespace MasterServer
{
    class Master: IMaster
    {
        private HttpListener HttpListener { get; set; }

        private IClientHandler ClientHandler { get; set; }

        public Master(IClientHandler clientHandler)
        {
            ClientHandler = clientHandler;
        }

        public void Start()
        {
            HttpListener = new HttpListener();
            HttpListener.Prefixes.Add(EnvironmentConstants.ServerHttpUrl);
            HttpListener.Start();
    
            ListenForConnections();
        }

        private void ListenForConnections()
        {
            while (true)
            {
                var httpListenerContext = HttpListener.GetContext();
                var thread = new Thread(HandleClient);
                thread.Start(httpListenerContext);
            }
        }

        private void HandleClient(object obj)
        {
            var httpContext = (HttpListenerContext) obj;
            ClientHandler.HandleClient(httpContext);
        }

        static void Main(string[] args)
        {
            var master = new Master(new ClientHandler());
            master.Start();
        }
    }
}
