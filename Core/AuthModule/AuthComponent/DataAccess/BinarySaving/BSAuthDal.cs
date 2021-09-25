namespace AppneuronUnity.Core.AuthModule.AuthComponent.DataAccess.BinarySaving
{
using AppneuronUnity.Core.AuthModule.AuthComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Concrete.BinaryType;

    /// <summary>
    /// Defines the <see cref="BSAuthDal" />.
    /// </summary>
    internal class BSAuthDal : BinaryTypeRepositoryBase<TokenDataModel>, IAuthDal
    {
    }
}
