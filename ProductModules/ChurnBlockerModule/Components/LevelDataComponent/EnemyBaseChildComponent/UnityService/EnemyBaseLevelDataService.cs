namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityService
{
    using AppneuronUnity.Core.UnityServices;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager;
    using System.Reflection;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;

    /// <summary>
    /// Defines the <see cref="EnemyBaseLevelDataService" />.
    /// </summary>
    public class EnemyBaseLevelDataService : MonoBehaviour
    {
        /// <summary>
        /// Defines the counterService.
        /// </summary>
        private CounterServices counterService;

        [Inject]
        private IEnemybaseLevelManager _enemybaseLevelManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private void Start()
        {

            counterService = GameObject.FindGameObjectWithTag("ChurnBlocker").GetComponent<CounterServices>();
        }

        /// <summary>
        /// The SendLevelData.
        /// </summary>
        /// <param name="charScores">The charScores<see cref="int"/>.</param>
        /// <param name="isFail">The isFail<see cref="bool"/>.</param>
        /// <param name="totalPowerUsage">The totalPowerUsage<see cref="int"/>.</param>
        /// <param name="chartransform">The chartransform<see cref="Transform"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendLevelData
            (int charScores,
            bool isFail,
            int totalPowerUsage,
            Transform chartransform)
        {
            await _enemybaseLevelManager.SendData(chartransform.position.x,
                chartransform.position.y,
                chartransform.position.z,
                isFail,
                charScores,
                totalPowerUsage,
                SceneManager.GetActiveScene().name,
                SceneManager.GetActiveScene().buildIndex,
                (int)counterService.LevelBaseGameTimer);
        }

        /// <summary>
        /// The SendLevelData.
        /// </summary>
        /// <param name="charScores">The charScores<see cref="int"/>.</param>
        /// <param name="isFail">The isFail<see cref="bool"/>.</param>
        /// <param name="chartransform">The chartransform<see cref="Transform"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendLevelData
            (int charScores,
            bool isFail,
            Transform chartransform)
        {
            await _enemybaseLevelManager.SendData(chartransform.position.x,
                chartransform.position.y,
                chartransform.position.z,
                isFail,
                charScores,
                0,
                SceneManager.GetActiveScene().name,
                SceneManager.GetActiveScene().buildIndex,
                (int)counterService.LevelBaseGameTimer);
        }

        /// <summary>
        /// The SendLevelData.
        /// </summary>
        /// <param name="charScores">The charScores<see cref="int"/>.</param>
        /// <param name="isFail">The isFail<see cref="bool"/>.</param>
        /// <param name="totalPowerUsage">The totalPowerUsage<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendLevelData
           (int charScores,
           bool isFail,
           int totalPowerUsage)
        {
            await _enemybaseLevelManager.SendData(0,
                0,
                0,
                isFail,
                charScores,
                totalPowerUsage,
                SceneManager.GetActiveScene().name,
                SceneManager.GetActiveScene().buildIndex,
                (int)counterService.LevelBaseGameTimer);
        }

        /// <summary>
        /// The SendLevelData.
        /// </summary>
        /// <param name="charScores">The charScores<see cref="int"/>.</param>
        /// <param name="isFail">The isFail<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendLevelData
          (int charScores,
          bool isFail)
        {
            await _enemybaseLevelManager.SendData(0,
                0,
                0,
                isFail,
                charScores,
                0,
                SceneManager.GetActiveScene().name,
                SceneManager.GetActiveScene().buildIndex,
                (int)counterService.LevelBaseGameTimer);
        }
    }
}
