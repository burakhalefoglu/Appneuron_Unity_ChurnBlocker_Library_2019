namespace AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataAccess
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Abstract;
using AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="IGameSessionEveryLoginDal" />.
    /// </summary>
    internal interface IGameSessionEveryLoginDal : IRepositoryService<GameSessionEveryLoginDataModel>
    {
    }
}
