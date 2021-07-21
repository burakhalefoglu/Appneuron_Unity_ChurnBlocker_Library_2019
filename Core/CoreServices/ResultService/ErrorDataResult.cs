using Assets.Appneuron.Core.CoreServices.ResultService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.CoreServices.ResultService
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data) : base(data, false)
        {

        }
        public ErrorDataResult(string message) : base(default, false, message)
        {

        }

        public ErrorDataResult(T data, string message) : base(data, false, message)
        {

        }
        public ErrorDataResult(T data, int statuseCode) : base(data, false, statuseCode)
        {

        }
        public ErrorDataResult(T data, int statuseCode, string message) : base(data, false, statuseCode, message)
        {

        }

    }
}
