using Appneuron.Zenject;
using AppneuronUnity.ProductModules.ChurnPrediction.WebSocketWorkers.RemoteChurnSettings.InterstitialAd.UnityWorker;
using AppneuronUnity.ProductModules.ChurnPrediction.Workers.RemoteChurnSettings.RemoteOffer;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd;

namespace AppneuronUnity.ProductModules.ChurnPrediction.IoC.Zenject
{
    public class ChurnPredictionBindingService : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInterstielAdUnityWorker>().To<InterstielAdUnityWorker>().AsSingle();
            Container.Bind<IRemoteOfferUnityWorker>().To<RemoteOfferUnityWorker>().AsSingle();

        }
    }
}
