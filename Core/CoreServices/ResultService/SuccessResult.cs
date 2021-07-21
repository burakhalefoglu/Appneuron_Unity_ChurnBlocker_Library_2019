using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.CoreServices.ResultService
{
    public class SuccessResult : Result
    {
        public SuccessResult() : base(true)
        {

        }
        public SuccessResult(string message) : base(true, message)
        {

        }

        public SuccessResult(int StatuseCode) : base(true, StatuseCode)
        {

        }

        public SuccessResult(string message, int StatuseCode) : base(true, message, StatuseCode)
        {

        }
    }
}
