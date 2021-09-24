namespace AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataAccess.BinarySaving
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Concrete.BinaryType;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataAccess;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataModel;

    /// <summary>
    /// Defines the <see cref="BSClientIdDal" />.
    /// </summary>
    internal class BSClientIdDal : BinaryTypeRepositoryBase<CustomerIdModel>, IClientIdDal
    {
    }
}
