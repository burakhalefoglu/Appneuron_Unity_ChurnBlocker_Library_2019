namespace AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataAccess.BinarySaving
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Concrete.BinaryType;
using AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="BSGameSessionEveryLoginDal" />.
    /// </summary>
    internal class BSGameSessionEveryLoginDal : BinaryTypeRepositoryBase<GameSessionEveryLoginDataModel>, IGameSessionEveryLoginDal
    {
    }
}
