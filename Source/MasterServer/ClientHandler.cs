using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

using MasterServer.Constants;
using MasterServer.Interfaces;

namespace MasterServer
{
    class ClientHandler : IClientHandler
    {

        private string userName;

        private string programCode;

        private string languageUsed;

        private string fullFilePath;

        public void HandleClient(HttpListenerContext httpContext)
        {
            ReadCodeFileFromClient(httpContext.Request);
            Console.WriteLine("File read from client");

            ICodeHandler codeHandler = new CodeHandler();

            var result = codeHandler.HandleCode(fullFilePath, languageUsed);

            ReturnResultToClient(httpContext.Response, result);
        }

        private void ReadCodeFileFromClient(HttpListenerRequest request)
        {
            using(var requestStream = request.InputStream)
            using(var streamReader = new StreamReader(requestStream))
            {
                this.userName = "user1";
                this.programCode = "foo";
                this.languageUsed = "C#";

                Console.WriteLine("Username = {0}, ProgramCode = {1}, LanguageUsed = {2}", userName, programCode, languageUsed);

                this.PrepareEnvironmentForUser();

                using (var fileWriter = new StreamWriter(fullFilePath))
                {
                    var data = streamReader.ReadLine();
                    while (data != null)
                    {
                        fileWriter.WriteLine(data);
                        data = streamReader.ReadLine();
                    }  
                }
                
            }
        }

        private void PrepareEnvironmentForUser()
        {
            var userDirectoryPath = FileConstants.CodeFilesPath + "\\" + userName;
            if (!Directory.Exists(userDirectoryPath))
            {
                Directory.CreateDirectory(userDirectoryPath);
            }

            fullFilePath = userDirectoryPath + "\\" + programCode + "_" + languageUsed + GetExtensionForLanguageUsed(languageUsed);
        }

        private void ReturnResultToClient(HttpListenerResponse httpResponse, IResult result)
        {
            using (var responseStream = httpResponse.OutputStream)
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(responseStream, result);
            }
        }

        private string GetExtensionForLanguageUsed(string language)
        {
            return ".cs";
        }
    }
}