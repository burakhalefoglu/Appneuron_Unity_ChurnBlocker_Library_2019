
using Assets.Appneuron.Core.CoreServices.DataStorageService.Concrete.BinaryType;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess.BinarySaving
{
    public class BSBuyingEventDal : BinaryTypeRepositoryBase<BuyingEventDataModel>, IBuyingEventDal
    {
    }
}

