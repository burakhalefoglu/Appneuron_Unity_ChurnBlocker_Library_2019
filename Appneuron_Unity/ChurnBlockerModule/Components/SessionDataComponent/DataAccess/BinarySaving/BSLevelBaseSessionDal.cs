
using Assets.Appneuron.Core.CoreServices.DataStorageService.Concrete.BinaryType;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataAccess.BinarySaving
{
    public class BSLevelBaseSessionDal : BinaryTypeRepositoryBase<LevelBaseSessionDataModel>, ILevelBaseSessionDal
    {
    }
}
