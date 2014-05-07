using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using MasterServer;
using MasterServer.Interfaces;

namespace TestClient
{
    public class Client
    {
        public string ServerUrl { get; set; }
        public int ServerPort { get; set; }

        Client(string serverUrl, int port)
        {
            this.ServerUrl = serverUrl;
            this.ServerPort = port;
        }

        static void Main(string[] args)
        {
            var client = new Client("http://localhost:8000?name=foo&password=bar", 8000);
            client.Start();
        }

        public void Start()
        {
            var response = SendFileToServer();
            Console.Write("File sent to server");
            var result = this.ReadResponseFromServer(response);

            PrintResponseToConsole(result);
            Console.ReadLine();
        }

        private HttpWebResponse SendFileToServer()
        {
            const string fileToUpload = @"C:\Problem1.cs";
            const string uploadUrl = "http://localhost:8000/";
            var encoder = new UTF8Encoding();

            var userName = encoder.GetBytes("user1\r\n");
            var programCode = encoder.GetBytes("problem1\r\n");
            var languageUsed = encoder.GetBytes("C#\r\n");

            var fileStream = new FileStream(fileToUpload, FileMode.Open);

            var httpWebRequest = WebRequest.Create(uploadUrl) as HttpWebRequest;
            httpWebRequest.Method = "POST"; // you might use "POST"
            httpWebRequest.ContentLength = fileStream.Length;
            httpWebRequest.AllowWriteStreamBuffering = true;

            Stream requestStream = httpWebRequest.GetRequestStream();

            var inData = new byte[fileStream.Length];

            // Get data from upload file to inData 
            fileStream.Read(inData, 0, int.Parse(fileStream.Length.ToString(CultureInfo.InvariantCulture)));

//            requestStream.Write(userName, 0, userName.Length);
//            requestStream.Write(programCode, 0, programCode.Length);
//            requestStream.Write(languageUsed, 0, languageUsed.Length);

            // put data into request stream
            requestStream.Write(inData, 0, (int)fileStream.Length);

            fileStream.Close();

            var response = httpWebRequest.GetResponse() as HttpWebResponse;

//            ReadResponseFromServer((HttpWebResponse)response);

            // after uploading close stream 
            requestStream.Close();

            return response;
        }

        private Result ReadResponseFromServer(HttpWebResponse httpResponse)
        {
            Console.WriteLine("Reading message from server");
            Result result;

            using (var stream = httpResponse.GetResponseStream())
            {
                var formatter = new BinaryFormatter();
                result = (Result)formatter.Deserialize(stream);
            }

            return result;
        }

        private void PrintResponseToConsole(Result result)
        {
            Console.WriteLine("---------------");
            Console.WriteLine("Results of your run:");

            if (result.RanSuccessfully)
            {
                Console.WriteLine("Congratulations! Your code ran successfully :)");
            }
            else if (result.HasCompilationErrors)
            {
                Console.WriteLine("The submitted code has compilation errors:");
                Console.WriteLine(result.ErrorMessage);
            }
            else if (result.FailedTestCase != null)
            {
                Console.WriteLine("Your solution failed in the following testcase");
                Console.WriteLine("Input: {0} \nExpected output: {1} \nReceived output: {2} \nException received: {3}", 
                    result.FailedTestCase.Input, result.FailedTestCase.ExpectedOutput, result.FailedTestCase.ReceivedOutput, string.IsNullOrEmpty(result.FailedTestCase.ExceptionThrown) ? "No exception thrown" : result.FailedTestCase.ExceptionThrown);
            }
            else if(!string.IsNullOrEmpty(result.ErrorMessage))
            {
                Console.WriteLine(result.ErrorMessage);
            }

            Console.WriteLine("------------");
        }
    }
}
