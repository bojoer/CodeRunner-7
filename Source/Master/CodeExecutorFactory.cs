using MasterServer.Interfaces;

namespace MasterServer
{
    public class CodeExecutorFactory
    {
        public static ICodeExecutor GetExecutor(string languageUsed)
        {
            if (languageUsed.Equals("JAVA"))
            {
//                return new JavaCodeTester();
            }

            if (languageUsed.Equals("C#"))
            {
                return new CSharpCodeExecutor();  
            }

            return null;
        }
    }
}
