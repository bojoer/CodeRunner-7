using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

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
            SendFileToServer();
            Console.Write("File sent to server");
            Console.ReadLine();
        }

        private void SendFileToServer()
        {
            const string fileToUpload = @"C:\Problem1.cs";
            const string uploadUrl = "http://localhost:8000/";
            var encoder = new UTF8Encoding();

            var userName = encoder.GetBytes("user1\r\n");
            var programCode = encoder.GetBytes("problem1\r\n");
            var languageUsed = encoder.GetBytes("C#\r\n");

            var fileStream = new FileStream(fileToUpload, FileMode.Open);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
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

            var response = httpWebRequest.GetResponse();

//            ReadResponseFromServer((HttpWebResponse)response);

            // after uploading close stream 
            requestStream.Close();
        }
    }
}
