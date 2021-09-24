namespace AppneuronUnity.Core.UnityComponents.InventoryComponent.DataAccess.BinarySaving
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Concrete.BinaryType;
using AppneuronUnity.Core.UnityComponents.InventoryComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="BSInventoryDal" />.
    /// </summary>
    internal class BSInventoryDal : BinaryTypeRepositoryBase<InventoryDataModel>, IInventoryDal
    {
    }
}
