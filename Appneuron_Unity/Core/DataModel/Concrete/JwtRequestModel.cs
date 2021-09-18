using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.DataModel.Concrete
{
    public class JwtRequestModel : IEntity
    {
        public string ClientId { get; set; }
        public string DashboardKey { get; set; }
        public string ProjectId { get; set; }
    }
}
