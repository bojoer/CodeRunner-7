namespace MasterServer.Interfaces
{
    public interface ICodeHandler
    {
        IResult HandleCode(string fileName, string languageUsed);
    }
}
