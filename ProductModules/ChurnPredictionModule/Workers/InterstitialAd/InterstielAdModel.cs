using AppneuronUnity.Core.Extentions;
using System.Collections.Generic;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd
{
    public class InterstielAdModel
    {
        public bool IsAdvSettingsActive { get; set; }
        public SerializableDictionary<string,int> DefaultIAdvFrequencyStrategy { get; set; }
    }
}
