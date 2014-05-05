using System;
using System.CodeDom.Compiler;
using System.IO;

using MasterServer.Constants;
using MasterServer.Interfaces;
using MasterServer.Utilities;

namespace MasterServer
{
    public class CSharpCodeExecutor : ICodeExecutor
    {
        public IResult Execute(string codeFilePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(codeFilePath);
            var testerFileName = this.GetTesterName(fileName);
            var testerFilePath = FileConstants.TesterFilesPath + "\\" + testerFileName;

            var compilerResultsForCode = CodeExecutionUtilities.Compile(codeFilePath);
            var compilerResultsForTester = CodeExecutionUtilities.Compile(testerFilePath);

            if (compilerResultsForCode.Errors.HasErrors)
            {
                return this.HandleCompilationErrors(compilerResultsForCode);
            }

            var solutionInstance = compilerResultsForCode.CompiledAssembly.CreateInstance("Problem1") as ISolution;

            var isSuccessful = (bool)CodeExecutionUtilities.Run(compilerResultsForTester, "MasterServer.TesterFiles.Problem1Tester" , "Test", new[] { solutionInstance as Object });

            Console.WriteLine("Run result:" + isSuccessful);

            return GetResultForTheExecutedCode(fileName);
        }

        private IResult GetResultForTheExecutedCode(string fileName)
        {
            return new Result();
        }

        private string GetTesterName(string fileName)
        {
            return fileName + "Tester.cs";
        }


        private IResult HandleCompilationErrors(CompilerResults compilerResults)
        {
            var result = new Result { IsSuccessFul = false };
            foreach (var error in compilerResults.Errors)
            {
                result.ErrorMessage += error;
            }

            return result;
        }
    }
}
