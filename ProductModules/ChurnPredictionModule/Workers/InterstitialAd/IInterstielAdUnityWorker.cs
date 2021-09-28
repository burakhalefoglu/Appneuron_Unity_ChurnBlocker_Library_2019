
using AppneuronUnity.Core.Extentions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd
{
    internal interface IInterstielAdUnityWorker
    {
        Task StartListen();
        Task GetInterstielFrequencyFromServer();
        SerializableDictionary<string, int> GetInterstielFrequency();
    }
}
