using MasterServer.Constants;
using MasterServer.Interfaces;

namespace MasterServer
{
    public class CodeHandler : ICodeHandler
    {
        public IResult HandleCode(string fileName, string languageUsed)
        {
            if (languageUsed == null)
            {
                return new Result
                {
                    IsSuccessFul = false,
                    ErrorMessage = ErrorMessages.UnableToDetectLanguage
                };
            }

            var codeExecutor = CodeExecutorFactory.GetExecutor(languageUsed);
            if (codeExecutor == null)
            {
                return new Result
                {
                    IsSuccessFul = false,
                    ErrorMessage = ErrorMessages.UnSupportedLanguage
                };
            }

            var result = codeExecutor.Execute(fileName);

            return result;
        }
    }
}
