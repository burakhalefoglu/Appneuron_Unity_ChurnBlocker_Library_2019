
using System;
using UnityEngine;

namespace AppneuronUnity.ProductModules.ChurnPrediction.Workers.RemoteChurnSettings.RemoteOffer
{
    public class RemoteOfferModel
    {
        public ProductModelDto[] ProductModelDtos { get; set; }
        public double FirstPrice { get; set; }
        public double LastPrice { get; set; }
        public bool IsGift { get; set; }
        public Texture GiftTexture { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }


    }
}
