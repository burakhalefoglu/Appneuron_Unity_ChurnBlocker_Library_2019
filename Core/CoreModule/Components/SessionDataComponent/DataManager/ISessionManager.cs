namespace AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataManager
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ISessionManager" />.
    /// </summary>
    internal interface ISessionManager
    {
        /// <summary>
        /// The SendLevelbaseSessionData.
        /// </summary>
        /// <param name="sessionSeconds">The sessionSeconds<see cref="float"/>.</param>
        /// <param name="levelName">The levelName<see cref="string"/>.</param>
        /// <param name="levelBaseGameSessionStart">The levelBaseGameSessionStart<see cref="DateTime"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendLevelbaseSessionData(float sessionSeconds,
                    string levelName,
                    int levelIndex,
                    DateTime levelBaseGameSessionStart);

        /// <summary>
        /// The CheckLevelBaseSessionDataAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CheckLevelBaseSessionDataAndSend();

        /// <summary>
        /// The SendGameSessionEveryLoginData.
        /// </summary>
        /// <param name="sessionStartTime">The sessionStartTime<see cref="DateTime"/>.</param>
        /// <param name="sessionFinishTime">The sessionFinishTime<see cref="DateTime"/>.</param>
        /// <param name="minutes">The minutes<see cref="float"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendGameSessionEveryLoginData(DateTime sessionStartTime,
            DateTime sessionFinishTime,
            float minutes);

        /// <summary>
        /// The CheckGameSessionEveryLoginDataAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CheckGameSessionEveryLoginDataAndSend();
    }
}
