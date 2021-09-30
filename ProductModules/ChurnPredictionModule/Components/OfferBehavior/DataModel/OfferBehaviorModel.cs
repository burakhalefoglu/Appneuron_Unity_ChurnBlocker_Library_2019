using System;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataModel
{
    internal class OfferBehaviorModel
    {

        public string ClientId { get; set; }
        public string ProjectId { get; set; }
        public string CustomerId { get; set; }
        public int OfferId { get; set; }

        private readonly DateTime DateTime = DateTime.Now;

        public bool isBuyOffer { get; set; }
    }
}
