
using AppneuronUnity.Core.Extentions;
using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd
{
    internal interface IInterstitialAdUnityWorker
    {
        Task StartListen();
        Task GetInterstitialFrequencyFromServer();
        SerializableDictionary<string, int> GetInterstitialFrequency();
    }
}
