
using System; 

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer
{
    public class RemoteOfferEventModel
    {
        public ProductModel[] ProductModels { get; set; }
        public float FirstPrice { get; set; }
        public float LastPrice { get; set; }
        public int OfferId { get; set; }
        public string Name { get; set; }
        public bool IsGift { get; set; }
        public byte[] GiftTexture { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }


    }
}
