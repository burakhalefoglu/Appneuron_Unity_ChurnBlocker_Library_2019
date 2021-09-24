namespace AppneuronUnity.Core.UnityComponents.BuyingDataComponent.DataAccess
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Abstract;
using AppneuronUnity.Core.UnityComponents.BuyingDataComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="IBuyingEventDal" />.
    /// </summary>
    internal interface IBuyingEventDal : IRepositoryService<BuyingEventDataModel>
    {
    }
}
