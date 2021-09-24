namespace AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataAccess
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Abstract;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataModel;

    /// <summary>
    /// Defines the <see cref="IClientIdDal" />.
    /// </summary>
    internal interface IClientIdDal : IRepositoryService<CustomerIdModel>
    {
    }
}
