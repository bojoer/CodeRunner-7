namespace MasterServer.Interfaces
{
    public interface ICodeExecutor
    {
        IResult Execute(string codeFilePath);
    }
}
