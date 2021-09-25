namespace AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataAccess
{
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataModel;
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;

    /// <summary>
    /// Defines the <see cref="IGameSessionEveryLoginDal" />.
    /// </summary>
    internal interface IGameSessionEveryLoginDal : IRepositoryService<GameSessionEveryLoginDataModel>
    {
    }
}
