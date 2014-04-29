using System;
using System.CodeDom.Compiler;
using System.Linq;

using Microsoft.CSharp;

namespace Spike_RunningCode
{
    public class Runner
    {
        static void Main(string[] args)
        {
            var runner = new Runner();

            var compilerResults = runner.Compile(@"C:\Projects\CodeRunner\TestClass.cs");

            runner.Run(compilerResults);

            Console.ReadLine();
        }

        public CompilerResults Compile(string filePath)
        {
            var parameters = new CompilerParameters
                                 {
                                     GenerateInMemory = true,
                                     TreatWarningsAsErrors = false,
                                     GenerateExecutable = false,
                                     CompilerOptions = "/optimize"
                                 };

            var codeProvider = new CSharpCodeProvider();
            var compilerResults = codeProvider.CompileAssemblyFromFile(parameters, new[] { filePath });

            return compilerResults;
        }

        public void Run(CompilerResults compilerResults)
        {
            if (compilerResults.Errors.HasErrors)
            {
                string errors = compilerResults.Errors.Cast<object>().Aggregate("Compilation errors: ", (current, error) => current + ("rn" + error));
                Console.WriteLine(errors);
                Console.ReadLine();
                return;
            }

            var module = compilerResults.CompiledAssembly.GetModules()[0];
            var moduleType = module.GetType("TestClass");
            var methodInfo = moduleType.GetMethod("Add");

            var returnValue = (int) methodInfo.Invoke(Activator.CreateInstance(moduleType), new object[] { 1, 2 });
            Console.WriteLine(returnValue);
        }
    }
}
