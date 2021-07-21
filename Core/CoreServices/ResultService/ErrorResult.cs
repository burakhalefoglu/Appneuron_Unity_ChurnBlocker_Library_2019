using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.CoreServices.ResultService
{
    public class ErrorResult : Result
    {
        public ErrorResult() : base(false)
        {

        }
        public ErrorResult(string message) : base(false, message)
        {

        }

        public ErrorResult(int StatuseCode) : base(false, StatuseCode)
        {

        }

        public ErrorResult(string message, int StatuseCode) : base(false, message, StatuseCode)
        {

        }


    }
}
