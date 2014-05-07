using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Xml;

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
            var testerFileName = FileUtilities.GetTesterFileName(fileName);
            var testerFilePath = FileUtilities.GetTesterFilePath(testerFileName);

            var compilerResultsForCode = CodeExecutionUtilities.Compile(codeFilePath);
            var compilerResultsForTester = CodeExecutionUtilities.Compile(testerFilePath);

            if (compilerResultsForCode.Errors.HasErrors)
            {
                return this.HandleCompilationErrors(compilerResultsForCode);
            }

            var solutionInstance = compilerResultsForCode.CompiledAssembly.CreateInstance("Problem1") as ISolution;

            return RunTesterForTheSolution(compilerResultsForTester, solutionInstance, codeFilePath);
        }

        private IResult RunTesterForTheSolution(CompilerResults compilerResultsForTester, ISolution solutionInstance, string codeFilePath)
        {
            var result = new Result();
            bool isSuccessful;

            try
            {
                isSuccessful = (bool)CodeExecutionUtilities.Run(
                                                                compilerResultsForTester, 
                                                                "MasterServer.TesterFiles.Problem1Tester", 
                                                                "Test", 
                                                                new[] { solutionInstance as Object, codeFilePath }
                                                                );
            }
            catch (Exception e)
            {
                result.RanSuccessfully = false;
                result.ErrorMessage = e.InnerException.ToString();

                return result;
            }

            return isSuccessful ? this.PrepareResultForSuccessfulRun() : PrepareResultForUnsuccessfulRun(codeFilePath);
        }

        private IResult PrepareResultForSuccessfulRun()
        {
            var result = new Result { RanSuccessfully = true, SuccessMessage = ErrorMessages.SuccessMessage };

            return result;
        } 

        private IResult PrepareResultForUnsuccessfulRun(string codeFilePath)
        {
            var result = new Result
                             {
                                 RanSuccessfully = false,
                                 FailedTestCase = this.GetFailedTestCaseInformation(FileUtilities.GetErrorFilePath(codeFilePath))
                             };

            return result;
        }

        private TestCaseOutput GetFailedTestCaseInformation(string errorFilePath)
        {
            // todo: Refactor this method to make it more clean
            var testCaseOutput = new TestCaseOutput();

            using (var xmlReader = XmlReader.Create(errorFilePath))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement() && !xmlReader.Name.Equals("FailedTestCase"))
                    {
                        string elementName = xmlReader.Name;
                        xmlReader.Read();
                        string textValueForElement = xmlReader.Value.Trim();
                        switch (elementName)
                        {
                            case "Input" :
                                testCaseOutput.Input = textValueForElement;
                                break;

                            case "ExpectedOutput" :
                                testCaseOutput.ExpectedOutput = textValueForElement;
                                break;

                            case "ReceivedOutput" :
                                testCaseOutput.ReceivedOutput = textValueForElement;
                                break;

                            case "ExceptionThrown" :
                                testCaseOutput.ExceptionThrown = textValueForElement;
                                break;
                        }
                    }
                }

            }

            return testCaseOutput;
        }

        

        private IResult HandleCompilationErrors(CompilerResults compilerResults)
        {
            var result = new Result { RanSuccessfully = false, HasCompilationErrors = true };
            foreach (var error in compilerResults.Errors)
            {
                result.ErrorMessage += error;
            }

            return result;
        }
    }
}
