namespace AppneuronUnity.Core.CoreModule.Components.LocationComponent.DataManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ILocationUnityManager" />.
    /// </summary>
    internal interface ILocationUnityManager
    {
        /// <summary>
        /// The SendMessage.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendMessage();
    }
}
