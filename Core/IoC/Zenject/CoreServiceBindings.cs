
namespace AppneuronUnity.Core.IoC.Zenject
{
    using AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact;
    using AppneuronUnity.Core.CoreServices.CryptoServices.Concrete.Effortless;
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.Core.CoreServices.MessageBrokers.Kafka;
    using AppneuronUnity.Core.CoreServices.RestClientServices.Abstract;
    using AppneuronUnity.Core.CoreServices.RestClientServices.Concrete.Unity;
    using AppneuronUnity.Core.UnityComponents.AdvDataComponent.DataAccess.BinarySaving;
    using AppneuronUnity.Core.UnityComponents.AdvDataComponent.DataAccess;
    using AppneuronUnity.Core.UnityComponents.AdvDataComponent.UnityManager;
    using AppneuronUnity.Core.UnityComponents.BuyingDataComponent.DataAccess.BinarySaving;
    using AppneuronUnity.Core.UnityComponents.BuyingDataComponent.DataAccess;
    using AppneuronUnity.Core.UnityComponents.BuyingDataComponent.UnityManager;
    using AppneuronUnity.Core.UnityComponents.HardwareIndormationComponent.UnityManager;
    using AppneuronUnity.Core.UnityComponents.InventoryComponent.DataAccess.BinarySaving;
    using AppneuronUnity.Core.UnityComponents.InventoryComponent.DataAccess;
    using AppneuronUnity.Core.UnityComponents.InventoryComponent.UnityManager;
    using AppneuronUnity.Core.UnityComponents.LocationComponent.UnityManager;
    using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.DataAccess.BSDal;
    using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.DataAccess;
    using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.UnityManager;
    using AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataAccess.BinarySaving;
    using AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataAccess;
    using AppneuronUnity.Core.UnityComponents.SessionDataComponent.UnityManager;
    using AppneuronUnity.Core.UnityWorkers.AuthWorker.DataAccess.BinarySaving;
    using AppneuronUnity.Core.UnityWorkers.AuthWorker.DataAccess;
    using AppneuronUnity.Core.UnityWorkers.AuthWorker.UnityManager;
    using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataAccess.BinarySaving;
    using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataAccess;
    using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;
    using global::Zenject;

    public class CoreServiceBindings : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICryptoServices>().To<EfforlessCryptoServices>().AsSingle();
            Container.Bind<IRestClientServices>().To<UnityRestApiService>().AsSingle();
            Container.Bind<IMessageBrokerService>().To<KafkaMessageBrokerService>().AsSingle();

            Container.Bind<IClientIdDal>().To<BSClientIdDal>().AsSingle();
            Container.Bind<IClientIdUnityManager>().To<ClientIdUnityManager>().AsSingle();

            Container.Bind<IAdvEventDal>().To<BSAdvEventDal>().AsSingle();
            Container.Bind<IAdvEventUnityManager>().To<AdvEventUnityManager>().AsSingle();

            Container.Bind<IAuthDal>().To<BSAuthDal>().AsSingle();
            Container.Bind<IAuthUnityManager>().To<AuthUnityManager>().AsSingle();

            Container.Bind<IBuyingEventDal>().To<BSBuyingEventDal>().AsSingle();
            Container.Bind<IBuyingEventManager>().To<BuyingEventManager>().AsSingle();

            Container.Bind<IGameSessionEveryLoginDal>().To<BSGameSessionEveryLoginDal>().AsSingle();
            Container.Bind<ILevelBaseSessionDal>().To<BSLevelBaseSessionDal>().AsSingle();
            Container.Bind<ISessionManager>().To<SessionManager>().AsSingle();

            Container.Bind<IInventoryDal>().To<BSInventoryDal>().AsSingle();
            Container.Bind<IInventoryUnityManager>().To<InventoryUnityManager>().AsSingle();

            Container.Bind<ISwipeScreenDataDal>().To<BSSwipeScreenDataDal>().AsSingle();
            Container.Bind<ISwipeDataUnityManager>().To<SwipeDataUnityManager>().AsSingle();

            Container.Bind<IClickDataDal>().To<BSClickDataDal>().AsSingle();
            Container.Bind<IClickDataUnityManager>().To<ClickDataUnityManager>().AsSingle();

            Container.Bind<IHardwareIndormationUnityManager>().To<HardwareIndormationUnityManager>().AsSingle();

            Container.Bind<ILocationUnityManager>().To<LocationUnityManager>().AsSingle();
        }
    }
}
