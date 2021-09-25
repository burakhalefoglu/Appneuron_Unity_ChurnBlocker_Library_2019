using Appneuron.Zenject;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.ProductModules.ChurnBlockerModule.IoC.Zenject;

namespace AppneuronUnity.ProductModules.ChurnPrediction.IoC.Zenject
{
    public class ChurnPredictionModuleBindings : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Container.Bind<IRemoteClient>().To<RemoteClient>().AsSingle();
            
        }
    }
}
