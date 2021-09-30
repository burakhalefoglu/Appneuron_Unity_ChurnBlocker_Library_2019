using AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataManager;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule
{
    public class ChurnPredictionModule:MonoBehaviour
    {

        [Inject]
        private IOfferBehaviorManager _offerBehaviorManager;

        public async Task SendOfferBehaviorData(int offerId, bool isBuyOffer)
        {
            await _offerBehaviorManager.SendAdvEventDataAsync(offerId, isBuyOffer);
        }

        public async Task<bool> GetOfferResultFromLocal(int OfferId)
        {
            //eğer bu id localde bulunan tüm verilerden büyükse true dönecek.(Reklam açılabilir)
            //eiğer durumda false dönecek. Açılamaz.
            return await _offerBehaviorManager.OfferIdIsInValidOnLocalAsync(OfferId);

        }
    }
}
