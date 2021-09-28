using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.ManuelFlowComponent.DataManager
{
    internal interface IManuelFlowDataManager
    {
        Task CheckAdvFileAndSendData();
        Task SendManuelFlowData(ManuelFlowModel manuelFlowModel);

    }
}
 