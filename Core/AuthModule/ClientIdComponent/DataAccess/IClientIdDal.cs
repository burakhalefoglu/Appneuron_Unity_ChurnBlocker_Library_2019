namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.DataAccess
{
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;

    /// <summary>
    /// Defines the <see cref="IClientIdDal" />.
    /// </summary>
    internal interface IClientIdDal : IRepositoryService<CustomerIdModel>
    {
    }
}
