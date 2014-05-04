using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
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

        public static string ExecuteCommand(string command)
        {
            var processStartInfo = new ProcessStartInfo {
                                           CreateNoWindow = true,
                                           UseShellExecute = false,
                                           RedirectStandardError = true,
                                           RedirectStandardOutput = true,
                                           WorkingDirectory = @"C:\",
                                           Arguments = @"/c csc C:\test.cs",
                                           FileName = "cmd.exe"
                                       };

            var process = Process.Start(processStartInfo);
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (output != null)
            {
                Console.WriteLine("Output:" + output);
            }

            if (error != null)
            {
                Console.WriteLine("Error:" + error);
            }

            return "";
        }
    }
}
