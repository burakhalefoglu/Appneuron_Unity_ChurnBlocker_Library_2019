namespace AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IBuyingEventManager" />.
    /// </summary>
    internal interface IBuyingEventManager
    {
        /// <summary>
        /// The CheckAdvFileAndSendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CheckAdvFileAndSendData();

        /// <summary>
        /// The SendAdvEventData.
        /// </summary>
        /// <param name="Tag">The Tag<see cref="string"/>.</param>
        /// <param name="levelName">The levelName<see cref="string"/>.</param>
        /// <param name="GameSecond">The GameSecond<see cref="float"/>.</param>
        /// <param name="clientId">The clientId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendAdvEventData(string Tag,
            string levelName,
            int levelIndex,
            float GameSecond,
            string clientId);
    }
}
