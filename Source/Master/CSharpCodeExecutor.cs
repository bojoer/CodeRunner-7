using System;
using System.IO;
using System.Runtime.Serialization;

using MasterServer.Interfaces;
using MasterServer.Utilities;

namespace MasterServer
{
    public class CSharpCodeExecutor : ICodeExecutor
    {
        public IResult Execute(string codeFilePath)
        {
            var fileName = Path.GetFileName(codeFilePath);
            var testerName = @"C:\Projects\CodeRunner\Source\Master\Problem1Tester.cs";

            var compilerResultsForCode = CodeExecutionUtilities.Compile(codeFilePath);
            var compilerResultsForTester = CodeExecutionUtilities.Compile(testerName);

            if (compilerResultsForCode.Errors.HasErrors)
            {
                var result = new Result { IsSuccessFul = false };
                foreach (var error in compilerResultsForCode.Errors)
                {
                    result.ErrorMessage += error;
                }

                return result;
            }

            var codeAssembly = compilerResultsForCode.CompiledAssembly;
            var assemblyType = codeAssembly.GetType();
            var solutionInstance = Activator.CreateInstance(typeof(Problem1));

            var isSuccessful = (bool)CodeExecutionUtilities.Run(compilerResultsForTester, "MasterServer.Problem1Tester" , "Test", new[] { solutionInstance });

            Console.WriteLine("Run result:" + isSuccessful);

            return GetResultForTheExecutedCode(fileName);
        }

        private IResult GetResultForTheExecutedCode(string fileName)
        {
            return new Result();
        }

        private string GetTesterName(string fileName)
        {
            return fileName + "_TESTER.cs";
        }
    }
}
