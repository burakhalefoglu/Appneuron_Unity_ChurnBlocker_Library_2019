using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnPrediction.Workers.RemoteChurnSettings.RemoteOffer
{
    public interface IRemoteOfferUnityWorker
    {
        Task StartListen();
        Task GetRemoteOfferFromServer();
        RemoteOfferModel GetRemoteOffer();

    }
}