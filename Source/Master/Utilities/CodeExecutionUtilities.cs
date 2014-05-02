using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

using MasterServer.Interfaces;

using Microsoft.CSharp;

namespace MasterServer.Utilities
{
    public class CodeExecutionUtilities
    {
        public static CompilerResults Compile(string filePath)
        {
            var parameters = new CompilerParameters
                                 {
                                     GenerateInMemory = true,
                                     TreatWarningsAsErrors = false,
                                     GenerateExecutable = false,
                                     CompilerOptions = "/optimize"
                                 };

            var assembly = GetExecutingAssemblyName();
            parameters.ReferencedAssemblies.Add(assembly);

            var codeProvider = new CSharpCodeProvider();
            var compilerResults = codeProvider.CompileAssemblyFromFile(parameters, new[] { filePath });

            return compilerResults;
        }

        public static object Run(CompilerResults compilerResults, string className, string methodName, object[] parameter)
        {
            var module = compilerResults.CompiledAssembly.GetModules()[0];
            var moduleType = module.GetType(className);
            var methodInfo = moduleType.GetMethod(methodName);

            return methodInfo.Invoke(Activator.CreateInstance(moduleType), parameter);
        }

        public static string GetExecutingAssemblyName()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);

            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetFileName(path); 
        }
    }
}
