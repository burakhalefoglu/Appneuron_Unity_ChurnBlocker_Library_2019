using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.BinarySaving;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess.BinarySaving;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataAccess.BinarySaving;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess.BinarySaving;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.DependencyInjectionResolverService.Ninject
{

    public class ChurnBlockerModuleBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IAdvEventDal>().To<BSAdvEventDal>().InSingletonScope();
            Bind<IBuyingEventDal>().To<BSBuyingEventDal>().InSingletonScope();
            Bind<IEnemyBaseWithLevelDieDal>().To<BSEnemyBaseWithLevelDieDal>().InSingletonScope();
            Bind<IEnemyBaseEveryLoginLevelDal>().To<BSEnemyBaseEveryLoginLevelDal>().InSingletonScope();
            Bind<IGameSessionEveryLoginDal>().To<BSGameSessionEveryLoginDal>().InSingletonScope();
            Bind<ILevelBaseSessionDal>().To<BSLevelBaseSessionDal>().InSingletonScope();

        }
    }
}
