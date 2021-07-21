using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.CoreServices.ResultService
{
    public interface IResult
    {
        int StatuseCode { get; }
        bool Success { get; }
        string Message { get; }
    }
}
