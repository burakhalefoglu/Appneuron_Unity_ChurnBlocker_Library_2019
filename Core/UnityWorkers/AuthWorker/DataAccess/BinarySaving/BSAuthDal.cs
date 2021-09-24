namespace AppneuronUnity.Core.UnityWorkers.AuthWorker.DataAccess.BinarySaving
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Concrete.BinaryType;
using AppneuronUnity.Core.UnityWorkers.AuthWorker.DataModel;

    /// <summary>
    /// Defines the <see cref="BSAuthDal" />.
    /// </summary>
    internal class BSAuthDal : BinaryTypeRepositoryBase<TokenDataModel>, IAuthDal
    {
    }
}
