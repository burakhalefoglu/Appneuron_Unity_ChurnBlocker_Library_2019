namespace AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataAccess
{
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;

    /// <summary>
    /// Defines the <see cref="IBuyingEventDal" />.
    /// </summary>
    internal interface IBuyingEventDal : IRepositoryService<BuyingEventDataModel>
    {
    }
}
