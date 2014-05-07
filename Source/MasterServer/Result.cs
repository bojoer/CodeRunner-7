using System;
using MasterServer.Interfaces;

namespace MasterServer
{
    [Serializable]
    public class Result : IResult
    {

        public Result()
        {
            FailedTestCase = null;
        }

        public bool RanSuccessfully { get; set; }

        public bool HasCompilationErrors { get; set; }

        public string ErrorMessage { get; set; }

        public string SuccessMessage { get; set; }

        public TestCaseOutput FailedTestCase { get; set; }
    }
}
