using System.IO;

using MasterServer.Interfaces;
using MasterServer.Utilities;

namespace MasterServer
{
    public class CSharpCodeExecutor : ICodeExecutor
    {
        public IResult Execute(string codeFilePath)
        {
            var fileName = Path.GetFileName(codeFilePath);
            var testerName = GetTesterName(fileName);

            var compilerResults = CodeExecutionUtilities.Compile(codeFilePath);
            if (compilerResults.Errors.HasErrors)
            {
                var result = new Result { IsSuccessFul = false };
                foreach (var error in compilerResults.Errors)
                {
                    result.ErrorMessage += error;
                }

                return result;
            }

            CodeExecutionUtilities.Run(compilerResults, testerName , "Test", new object[] { fileName });

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
