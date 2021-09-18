using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.DataModel.Concrete
{
    public class JwtResponseModel
    {
        public TokenDataModel Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
