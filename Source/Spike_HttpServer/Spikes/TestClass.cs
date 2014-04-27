using System;

namespace Spike_HttpServer.Spikes
{
    [Serializable]
    public class TestClass
    {
        public string Property1 { get; set; }

        public int Property2 { get; set; }

        public void SayHello()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
