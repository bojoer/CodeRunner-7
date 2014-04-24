using MasterServer.Interfaces;

namespace MasterServer
{
    public class Result : IResult
    {
        public bool IsSuccessFul { set; get; }

        public string ErrorMessage { set; get; }
    }
}
