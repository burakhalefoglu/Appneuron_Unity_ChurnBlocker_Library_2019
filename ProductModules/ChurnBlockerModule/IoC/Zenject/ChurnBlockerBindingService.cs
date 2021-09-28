using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess.BinarySaving;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess.BinarySaving;
using Zenject;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataManager;
using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.Helper;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.ManuelFlowComponent.DataManager;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.DataManager;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.IoC.Zenject
{
    public class ChurnBlockerBindingService : Installer<ChurnBlockerBindingService>
    {
        public override void InstallBindings()
        {

            Container.Bind<IEnemyBaseWithLevelDieDal>().To<BSEnemyBaseWithLevelDieDal>().AsSingle();
            Container.Bind<IEnemyBaseEveryLoginLevelDal>().To<BSEnemyBaseEveryLoginLevelDal>().AsSingle();
            Container.Bind<IEnemybaseLevelManager>().To<EnemybaseLevelManager>().AsSingle();

            Container.Bind<IDifficultyDal>().To<BSDifficultyDal>().AsSingle();
            Container.Bind<IDifficultyDataManager>().To<DifficultyDataManager>().AsSingle();
            
            Container.Bind<IManuelFlowDal>().To<BSManuelFlowDal>().AsSingle();
            Container.Bind<IManuelFlowDataManager>().To<ManuelFlowDataManager>().AsSingle();

            Container.Bind<IDifficultyResultUnityWorker>().To<DifficultyResultUnityWorker>().AsSingle();

            Container.Bind<DifficultyInternalModel>().AsSingle();
            Container.Bind<CharInfo>().AsSingle();
            Container.Bind<DifficultyHelper>().AsSingle();

            

        }
    }
}
