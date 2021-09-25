namespace AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataAccess
{
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;

    /// <summary>
    /// Defines the <see cref="IInventoryDal" />.
    /// </summary>
    internal interface IInventoryDal : IRepositoryService<InventoryDataModel>
    {
    }
}
