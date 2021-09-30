using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataManager
{
    internal interface IOfferBehaviorManager
    {
        Task CheckFileAndSendDataAsync();
        Task SendAdvEventDataAsync(int OfferId,
           bool isBuyOffer);
        Task<bool> OfferIdIsInValidOnLocalAsync(int OfferId);
    }
}
