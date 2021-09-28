namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IClientIdUnityManager" />.
    /// </summary>
    internal interface IClientIdUnityManager
    {
        string GetPlayerID();
        string GenerateId();
    }
}
