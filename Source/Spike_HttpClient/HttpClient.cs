using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Spike_HttpClient
{
    public class HttpClient
    {
        private string ServerUrl { set; get; }

        public HttpClient(string serverUrl, int serverPort)
        {
            ServerUrl = serverUrl;
        }

        static void Main(string[] args)
        {
            var client = new HttpClient("http://localhost:8000?name=foo&password=bar", 8000);
            client.Start();
        }

        public void Start()
        {
//            TalkToServer();
            SendFileToServer();
            Console.ReadLine();
        }

        private void TalkToServer()
        {
            var httpWebRequest = WebRequest.Create(new Uri(ServerUrl)) as HttpWebRequest;
            httpWebRequest.Method = WebRequestMethods.Http.Get;

            //            var parameters = GetParameters();
            //            var urlParameters = PrepareUrlParameterList(parameters);

            string dataFromServer = "";

            try
            {
                var httpResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                Console.WriteLine("Reading from server's response");

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    dataFromServer = ReadResponseFromServer(httpResponse);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An exception in client:" + e.Message);
                return;
            }

            Console.WriteLine(dataFromServer);

            Console.ReadLine();
        }

        private string ReadResponseFromServer(HttpWebResponse httpResponse)
        {
            string responseMessage;
            Console.WriteLine("Reading message from server");

            using (var responseStream = httpResponse.GetResponseStream())
            {
                using (var reader = new StreamReader(responseStream))
                {
                    responseMessage = reader.ReadToEnd();
                }
            }

            return responseMessage;
        }

        private void SendFileToServer()
        {
            const string fileToUpload = @"C:\Users\Rohini\My Documents\test.txt";
            const string uploadUrl = "http://localhost:8000/";

            var fileStream = new FileStream(fileToUpload, FileMode.Open);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            httpWebRequest.Method = "POST"; // you might use "POST"
            httpWebRequest.ContentLength = fileStream.Length;
            httpWebRequest.AllowWriteStreamBuffering = true;

            Stream requestStream = httpWebRequest.GetRequestStream();

            var inData = new byte[fileStream.Length];

            // Get data from upload file to inData 
            fileStream.Read(inData, 0, int.Parse(fileStream.Length.ToString(CultureInfo.InvariantCulture)));

            // put data into request stream
            requestStream.Write(inData, 0, int.Parse(fileStream.Length.ToString(CultureInfo.InvariantCulture)));

            fileStream.Close();

            var response = httpWebRequest.GetResponse();

            var messageFromServer = ReadResponseFromServer((HttpWebResponse)response);
            Console.WriteLine(messageFromServer);

            // after uploading close stream 
            requestStream.Close();
        }

        private String PrepareUrlParameterList(IDictionary<string, string> parameters)
        {
            var urlParameters = new StringBuilder();

            foreach (var parameter in parameters)
            {
                if (urlParameters.Length > 0)
                {
                    urlParameters.Append("&");
                }

                urlParameters.Append(parameter.Key + "=" + Uri.EscapeUriString(parameter.Value));
            }

            return urlParameters.ToString();
        }

        private IDictionary<string, string> GetParameters()
        {
            return new Dictionary<string, string>
            {
                { "Username", "foo"},
                { "Password", "bar"}
            };
        }

    }
}
