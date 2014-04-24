using MasterServer.Constants;
using MasterServer.Interfaces;

namespace MasterServer
{
    public class CodeRunner : ICodeRunner
    {
        public IResult Run(string fileName)
        {

            var languageUsed = GetLanguageOfTheCode(fileName);
            if (languageUsed == null)
            {
                return new Result
                {
                    IsSuccessFul = false,
                    ErrorMessage = ErrorMessages.UnableToDetectLanguage
                };
            }

            var codeTester = CodeTesterFactory.GetTester(languageUsed);
            if (codeTester == null)
            {
                return new Result
                {
                    IsSuccessFul = false,
                    ErrorMessage = ErrorMessages.UnSupportedLanguage
                };
            }

            var result = codeTester.Test(fileName);

            return result;
        }

        private string GetLanguageOfTheCode(string fileName)
        {
            return null;
        }
    }
}