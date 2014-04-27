using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.SqlServer.Server;
using Spike_HttpServer.Spikes;

namespace Spike_HttpServer
{
    class HttpServer
    {
        private HttpListener httpListener;
        private const string ServerUrl = "http://localhost:8000/";

        static void Main()
        {
            var server = new HttpServer();
            server.Start();

        }

        public void Start()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(ServerUrl);
            httpListener.Start();

            ListenForConnections();
        }

        private void ListenForConnections()
        {
            while (true)
            {
                var httpListenerContext = httpListener.GetContext();
                var thread = new Thread(HandleClient);
                thread.Start(httpListenerContext);
            }
        }

        private void HandleClient(object obj)
        {
            var httpListenerContext = (HttpListenerContext) obj;

            ReadFileFromClient(httpListenerContext);

//            WriteToClient(httpListenerContext.Response, "You are connected and your file has been received!");

            WriteObjectToClient(httpListenerContext.Response, new TestClass { Property1 = "Hello World!", Property2 = 42 });
        }

        private void ReadFileFromClient(HttpListenerContext context)
        {

            using (var stream = context.Request.InputStream)
            using (var reader = new StreamReader(stream))
            using (var fileWriter = new StreamWriter("d:\\test.txt"))
            {
                var data = reader.ReadLine();
                while (data != null)
                {
                    fileWriter.WriteLine(data);
                    data = reader.ReadLine();
                }
            }

            Console.WriteLine("Finished reading the file from client");
        }

        private void WriteToClient(HttpListenerResponse httpResponse, string responseString)
        {
            byte[] responseContents = Encoding.UTF8.GetBytes(responseString);

            httpResponse.ContentLength64 = responseContents.Length;

            using (var outputStream = httpResponse.OutputStream)
            {
                outputStream.Write(responseContents, 0, responseContents.Length);
            }
        }

        private void WriteObjectToClient(HttpListenerResponse httpResponse, TestClass testObject)
        {
            testObject.Property1 = "Hello World!";
            testObject.Property2 = 42;

            using (var stream = httpResponse.OutputStream)
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, testObject);
            }
        }
    }
}
