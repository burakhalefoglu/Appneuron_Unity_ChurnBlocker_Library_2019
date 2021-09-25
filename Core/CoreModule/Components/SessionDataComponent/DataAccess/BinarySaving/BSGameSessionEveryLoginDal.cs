namespace AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataAccess.BinarySaving
{
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Concrete.BinaryType;

    /// <summary>
    /// Defines the <see cref="BSGameSessionEveryLoginDal" />.
    /// </summary>
    internal class BSGameSessionEveryLoginDal : BinaryTypeRepositoryBase<GameSessionEveryLoginDataModel>, IGameSessionEveryLoginDal
    {
    }
}
