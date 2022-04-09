using System;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataModel
{
    internal class OfferBehaviorModel
    {
         
        public long ClientId { get; set; }
        public long ProjectId { get; set; }
        public long CustomerId { get; set; }
        public int OfferId { get; set; }
        public string OfferName { get; set; }

        private readonly DateTime dateTime = DateTime.Now;
        public int IsBuyOffer { get; set; }
    }
}
