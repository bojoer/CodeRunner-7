using System;

namespace MasterServer.Interfaces
{
    public interface ITester
    {
        bool Test(Object solution, string codeFilePath);
    }
}
