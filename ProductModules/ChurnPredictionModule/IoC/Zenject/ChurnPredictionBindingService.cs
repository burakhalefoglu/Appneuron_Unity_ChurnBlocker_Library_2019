using Zenject;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd;
using AppneuronUnity.ProductModules.ChurnPredictionModule.IoC.Zenject;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.IoC.Zenject
{
    public class ChurnPredictionBindingService : Installer<ChurnPredictionBindingService>
    {
        public override void InstallBindings()
        {
            Container.Bind<IInterstielAdUnityWorker>().To<InterstielAdUnityWorker>().AsSingle();
            Container.Bind<IRemoteOfferUnityWorker>().To<RemoteOfferUnityWorker>().AsSingle();

        }
    }
}
