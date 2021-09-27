
using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd
{
    internal interface IInterstielAdUnityWorker
    {
        Task StartListen();
        Task GetInterstielFrequencyFromServer();
        int GetInterstielFrequency();
    }
}
