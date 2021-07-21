using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appneuron.Core.DataModel.Concrete
{
    public class TokenSingletonModel
    {
        private static readonly TokenSingletonModel instance = new TokenSingletonModel();

        static TokenSingletonModel()
        {
        }

        private TokenSingletonModel()
        {
        }

        public static TokenSingletonModel Instance
        {
            get
            {
                return instance;
            }
        }


        public string Token { get; set; }
    }
}
