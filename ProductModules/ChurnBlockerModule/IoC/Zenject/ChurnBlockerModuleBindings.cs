using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess.BinarySaving;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.UnityManager;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess.BinarySaving;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager;
using Zenject;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.IoC.Zenject
{
    public class ChurnBlockerModuleBindings : MonoInstaller
    {
        public override void InstallBindings()
        {

            Container.Bind<IEnemyBaseWithLevelDieDal>().To<BSEnemyBaseWithLevelDieDal>().AsSingle();
            Container.Bind<IEnemyBaseEveryLoginLevelDal>().To<BSEnemyBaseEveryLoginLevelDal>().AsSingle();
            Container.Bind<IEnemybaseLevelManager>().To<EnemybaseLevelManager>().AsSingle();

            Container.Bind<IDifficultyLevelDal>().To<BSDifficultyLevelDal>().AsSingle();
            Container.Bind<IDifficultyManager>().To<DifficultyManager>().AsSingle();
        }
    }
}
