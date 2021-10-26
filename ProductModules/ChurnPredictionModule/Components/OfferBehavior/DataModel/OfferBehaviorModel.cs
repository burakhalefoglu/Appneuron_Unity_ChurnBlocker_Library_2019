using System;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataModel
{
    internal class OfferBehaviorModel
    {
         
        public string ClientId { get; set; }
        public string ProjectId { get; set; }
        public string CustomerId { get; set; }
        public int OfferId { get; set; }
        public string OfferName { get; set; }

        private readonly DateTime dateTime = DateTime.Now;
        public int IsBuyOffer { get; set; }
    }
}
