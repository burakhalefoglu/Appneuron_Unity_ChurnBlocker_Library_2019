using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.DataModelBase.Concrete
{

    [Serializable]
    public class CustomerIdModel : IEntity
    {
        public string _id { get; set; }
    }

}