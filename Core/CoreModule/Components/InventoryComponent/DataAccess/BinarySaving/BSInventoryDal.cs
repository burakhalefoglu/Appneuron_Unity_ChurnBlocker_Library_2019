namespace AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataAccess.BinarySaving
{
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Concrete.BinaryType;

    /// <summary>
    /// Defines the <see cref="BSInventoryDal" />.
    /// </summary>
    internal class BSInventoryDal : BinaryTypeRepositoryBase<InventoryDataModel>, IInventoryDal
    {
    }
}
