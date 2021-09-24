namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.UnityManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IDifficultyManager" />.
    /// </summary>
    internal interface IDifficultyManager
    {
        /// <summary>
        /// The AskDifficulty.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task AskDifficulty();
    }
}
