namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IClientIdUnityManager" />.
    /// </summary>
    internal interface IClientIdUnityManager
    {
        /// <summary>
        /// The SaveIdOnLocalStorage.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SaveIdOnLocalStorage();

        /// <summary>
        /// The GetPlayerID.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        string GetPlayerID();

        /// <summary>
        /// The GenerateId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        string GenerateId();
    }
}
