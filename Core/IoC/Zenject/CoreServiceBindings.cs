namespace AppneuronUnity.Core.IoC.Zenject
{
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.AuthModule.AuthComponent.DataAccess;
    using AppneuronUnity.Core.AuthModule.AuthComponent.DataAccess.BinarySaving;
    using AppneuronUnity.Core.AuthModule.AuthComponent.UnityManager;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataAccess;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataAccess.BinarySaving;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
    using global::Appneuron.Zenject;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Concrete.Effortless;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Concrete.Unity;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataAccess.BinarySaving;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataAccess.BinarySaving;
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataAccess.BinarySaving;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.LocationComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataAccess.BSDal;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataAccess.BinarySaving;
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.UnityManager;

    public class CoreServiceBindings : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICryptoServices>().To<EfforlessCryptoServices>().AsSingle();
            Container.Bind<IRestClientServices>().To<UnityRestApiService>().AsSingle();
            Container.Bind<IRemoteClient>().To<RemoteClient>().AsSingle();
            Container.Bind<IDataCreationClient>().To<DataCreationClient>().AsSingle();

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
