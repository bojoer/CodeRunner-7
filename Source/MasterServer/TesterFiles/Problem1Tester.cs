using System;
using System.Globalization;

using MasterServer.Interfaces;
using MasterServer.Utilities;

namespace MasterServer.TesterFiles
{
    public class Problem1Tester : ITester
    {
        public bool Test(Object soln, string codeFilePath)
        {
            IProblem1 solution = soln as IProblem1;
            int sum = solution.Add(1, 2);
            bool flag = (sum == 3);

            if (!flag)
            {
                WriteErrorToXml(codeFilePath, "1 2", "3", sum.ToString(CultureInfo.InvariantCulture), "");
            }

            return flag;
        }

        private void WriteErrorToXml(string codeFilePath, string input, string expectedOutput, string receivedOutput, string exceptionThrown)
        {
            string errorFilePath = FileUtilities.GetErrorFilePath(codeFilePath);
            var failedTestCase = new TestCaseOutput
                                     {
                                         Input = input,
                                         ExpectedOutput = expectedOutput,
                                         ReceivedOutput = receivedOutput,
                                         ExceptionThrown = exceptionThrown
                                     };

            FileUtilities.WriteFailedTestCaseToXmlFile(failedTestCase, errorFilePath);
        }
    }
}
