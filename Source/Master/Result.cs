using System;
using MasterServer.Interfaces;

namespace MasterServer
{
    [Serializable]
    public class Result : IResult
    {
        public bool IsSuccessFul { set; get; }

        public string ErrorMessage { set; get; }
    }
}
