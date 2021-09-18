using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.CoreServices.ResultService
{
    public class Result : IResult
    {
        public Result(bool success)
        {
            Success = success;

        }
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }

        public Result(bool success, string message, int statuseCode) : this(success, message)
        {
            StatuseCode = statuseCode;
        }

        public Result(bool success, int statuseCode) : this(success)
        {
            StatuseCode = statuseCode;
        }

        public bool Success { get; }
        public string Message { get; }
        public int StatuseCode { get; }

    }
}
