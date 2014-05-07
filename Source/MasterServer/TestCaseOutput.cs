using System;

namespace MasterServer
{
    [Serializable]
    public class TestCaseOutput
    {
        public string Input { get; set; }

        public string ExpectedOutput { get; set; }

        public string ReceivedOutput { get; set; }

        public string ExceptionThrown { get; set; }
    }
}
