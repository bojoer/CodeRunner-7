using MasterServer.Interfaces;

namespace MasterServer
{
    public class CodeTesterFactory
    {
        public static ICodeTester GetTester(string languageUsed)
        {
            if (languageUsed.Equals("JAVA"))
            {
//                return new JavaCodeTester();
            }

            if (languageUsed.Equals("C#"))
            {
//                return new CSharpCodeTester();
            }

            return null;
        }
    }
}
