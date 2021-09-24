namespace AppneuronUnity.Core.UnityWorkers.AuthWorker.DataAccess
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Abstract;
using AppneuronUnity.Core.UnityWorkers.AuthWorker.DataModel;

    /// <summary>
    /// Defines the <see cref="IAuthDal" />.
    /// </summary>
    internal interface IAuthDal : IRepositoryService<TokenDataModel>
    {
    }
}
