using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using MasterServer.Interfaces;

namespace MasterServer
{
    class ClientHandler : IClientHandler
    {
        private const string FileName = "D:\\test.txt";

        public void HandleClient(HttpListenerContext httpContext)
        {

            var fileName = ReadCodeFileFromClient(httpContext.Request);

            ICodeHandler codeHandler = new CodeHandler();

            var result = codeHandler.Execute(fileName);

            ReturnResultToClient(httpContext.Response, result);
        }

        private string ReadCodeFileFromClient(HttpListenerRequest request)
        {
            using(var requestStream = request.InputStream)
            using(var streamReader = new StreamReader(requestStream))
            using (var fileWriter = new StreamWriter(FileName))
            {
                string data = streamReader.ReadLine();
                while (data != null)
                {
                    fileWriter.WriteLine(data);
                    data = streamReader.ReadLine();
                }
            }

            return FileName;
        }

        private void ReturnResultToClient(HttpListenerResponse httpResponse, IResult result)
        {
            using (var responseStream = httpResponse.OutputStream)
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(responseStream, result);
            }
        }
    }
}