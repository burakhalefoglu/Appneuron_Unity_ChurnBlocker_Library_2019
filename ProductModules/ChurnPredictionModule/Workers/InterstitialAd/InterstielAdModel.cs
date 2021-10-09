using AppneuronUnity.Core.Extentions;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd
{
    public class InterstielAdModel
    {
        public bool IsAdvSettingsActive { get; set; }
        public SerializableDictionary<string,int> AdvFrequencyStrategies { get; set; }
    }
}
