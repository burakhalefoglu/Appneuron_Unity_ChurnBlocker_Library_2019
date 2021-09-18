using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.DataModel.Concrete
{
    [Serializable]
    public class TokenDataModel : IEntity
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
