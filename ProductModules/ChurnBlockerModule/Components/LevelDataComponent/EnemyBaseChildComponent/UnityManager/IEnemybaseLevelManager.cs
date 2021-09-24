namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IEnemybaseLevelManager" />.
    /// </summary>
    internal interface IEnemybaseLevelManager
    {
        /// <summary>
        /// The SendData.
        /// </summary>
        /// <param name="TransformX">The TransformX<see cref="float"/>.</param>
        /// <param name="TransformY">The TransformY<see cref="float"/>.</param>
        /// <param name="TransformZ">The TransformZ<see cref="float"/>.</param>
        /// <param name="IsDead">The IsDead<see cref="bool"/>.</param>
        /// <param name="AverageScores">The AverageScores<see cref="int"/>.</param>
        /// <param name="TotalPowerUsage">The TotalPowerUsage<see cref="int"/>.</param>
        /// <param name="sceneName">The sceneName<see cref="string"/>.</param>
        /// <param name="İnTime">The İnTime<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendData
           (float TransformX,
           float TransformY,
           float TransformZ,
           bool IsDead,
           int AverageScores,
           int TotalPowerUsage, string sceneName,
           int levelIndex,
           int İnTime);

        /// <summary>
        /// The CheckLevelbaseDieAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CheckLevelbaseDieAndSend();

        /// <summary>
        /// The CheckEveryLoginLevelDatasAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CheckEveryLoginLevelDatasAndSend();
    }
}
