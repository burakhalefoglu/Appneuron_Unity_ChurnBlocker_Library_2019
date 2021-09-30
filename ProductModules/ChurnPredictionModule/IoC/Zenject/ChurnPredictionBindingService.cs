using Zenject;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd;
using AppneuronUnity.ProductModules.ChurnPredictionModule.IoC.Zenject;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataAccess.BinarySaving;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataAccess;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataManager;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.IoC.Zenject
{
    public class ChurnPredictionBindingService : Installer<ChurnPredictionBindingService>
    {
        public override void InstallBindings()
        {
            Container.Bind<IInterstielAdUnityWorker>().To<InterstielAdUnityWorker>().AsSingle();
            Container.Bind<IRemoteOfferUnityWorker>().To<RemoteOfferUnityWorker>().AsSingle();

            Container.Bind<IOfferBehaviorDal>().To<BSOfferBehaviorDal>().AsSingle();
            Container.Bind<IOfferBehaviorManager>().To<OfferBehaviorManager>().AsSingle();
            
        }
    }
}
