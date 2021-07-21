using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.CoreServices.ResultService
{
    public class DataResult<T> : IDataResult<T>
    {

        public DataResult(T data, bool success)
        {
            Data = data;
            Success = success;
        }

        public DataResult(T data, bool success, string message) : this(data, success)
        {
            Message = message;
        }

        public DataResult(T data, bool success, int statuseCode) : this(data, success)
        {
            StatuseCode = statuseCode;
        }

        public DataResult(T data, bool success, int statuseCode, string message) : this(data, success, message)
        {
            StatuseCode = statuseCode;
        }

        public T Data { get; }
        public bool Success { get; }
        public string Message { get; }
        public int StatuseCode { get; }
    }
}
