namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.DataAccess.BinarySaving
{
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataAccess;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Concrete.BinaryType;

    /// <summary>
    /// Defines the <see cref="BSClientIdDal" />.
    /// </summary>
    internal class BSClientIdDal : BinaryTypeRepositoryBase<CustomerIdModel>, IClientIdDal
    {
    }
}
