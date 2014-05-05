using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CSharp;

namespace Spike_RunningCode
{
    public class Runner
    {
        static void Main(string[] args)
        {
            Console.WriteLine(typeof(Runner).Namespace);
            Console.ReadLine();
        }

        public CompilerResults Compile(string filePath)
        {
            var parameters = new CompilerParameters
                                 {
                                     GenerateInMemory = true,
                                     TreatWarningsAsErrors = false,
                                     GenerateExecutable = true,
                                     CompilerOptions = "/optimize"
                                 };

            var codeProvider = new CSharpCodeProvider();
            var compilerResults = codeProvider.CompileAssemblyFromFile(parameters, new[] { filePath });
            compilerResults.PathToAssembly = @"C:\";

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
            var moduleType = module.GetType("test");
            var methodInfo = moduleType.GetMethod("Main");

            var returnValue = (int) methodInfo.Invoke(null, new object[] { null });
            Console.WriteLine(returnValue);
        }
    }
}
