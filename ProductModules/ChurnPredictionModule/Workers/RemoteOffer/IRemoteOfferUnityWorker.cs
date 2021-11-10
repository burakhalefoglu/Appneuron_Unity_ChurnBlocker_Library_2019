using System.Threading.Tasks;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer
{
    public interface IRemoteOfferUnityWorker
    {
        Task StartListen();
        Task GetRemoteOfferFromServer();
        RemoteOfferEventModel GetRemoteOffer();

    }
}
