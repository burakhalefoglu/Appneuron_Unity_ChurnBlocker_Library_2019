namespace AppneuronUnity.Core.AuthModule.AuthComponent.DataAccess
{
using AppneuronUnity.Core.AuthModule.AuthComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;

    /// <summary>
    /// Defines the <see cref="IAuthDal" />.
    /// </summary>
    internal interface IAuthDal : IRepositoryService<TokenDataModel>
    {
    }
}
