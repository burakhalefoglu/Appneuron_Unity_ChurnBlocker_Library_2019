namespace AppneuronUnity.Core.UnityComponents.InventoryComponent.DataAccess
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Abstract;
using AppneuronUnity.Core.UnityComponents.InventoryComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="IInventoryDal" />.
    /// </summary>
    internal interface IInventoryDal : IRepositoryService<InventoryDataModel>
    {
    }
}
