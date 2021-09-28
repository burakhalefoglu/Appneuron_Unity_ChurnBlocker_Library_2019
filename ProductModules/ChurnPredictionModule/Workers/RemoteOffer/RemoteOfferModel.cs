
using System;
using UnityEngine;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer
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
