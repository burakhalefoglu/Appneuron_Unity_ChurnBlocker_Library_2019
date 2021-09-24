namespace AppneuronUnity.Core.UnityServices
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Defines the <see cref="CounterServices" />.
    /// </summary>
    public class CounterServices : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the LevelBaseGameTimer.
        /// </summary>
        [HideInInspector]
        public float LevelBaseGameTimer { get; set; }

        /// <summary>
        /// Gets or sets the TimerForGeneralSession.
        /// </summary>
        [HideInInspector]
        public float TimerForGeneralSession { get; set; }

        /// <summary>
        /// Gets or sets the LevelBaseGameSessionStart.
        /// </summary>
        [HideInInspector]
        public DateTime LevelBaseGameSessionStart { get; set; }

        /// <summary>
        /// Gets or sets the GameSessionEveryLoginStart.
        /// </summary>
        [HideInInspector]
        public DateTime GameSessionEveryLoginStart { get; set; }

        /// <summary>
        /// The Start.
        /// </summary>
        private void Start()
        {
            LevelBaseGameTimer = 0;
            GameSessionEveryLoginStart = DateTime.Now;
            LevelBaseGameSessionStart = DateTime.Now;
        }

        /// <summary>
        /// The Update.
        /// </summary>
        private void Update()
        {
            LevelBaseGameTimer += Time.deltaTime;
        }

        /// <summary>
        /// The OnEnable.
        /// </summary>
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// The OnSceneLoaded.
        /// </summary>
        /// <param name="scene">The scene<see cref="Scene"/>.</param>
        /// <param name="mode">The mode<see cref="LoadSceneMode"/>.</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LevelBaseGameTimer = 0;
            LevelBaseGameSessionStart = DateTime.Now;
        }
    }
}
