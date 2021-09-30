using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataModel;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataAccess
{
    internal interface IOfferBehaviorDal : IRepositoryService<OfferBehaviorModel>
    {
    }
}
