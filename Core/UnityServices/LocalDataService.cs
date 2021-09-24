namespace AppneuronUnity.Core.UnityServices
{
    using System.Threading.Tasks;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="LocalDataService" />.
    /// </summary>
    public class LocalDataService : MonoBehaviour
    {
        /// <summary>
        /// Defines the connectionService.
        /// </summary>
        internal ConnectionService connectionService;

        /// <summary>
        /// Defines the timer.
        /// </summary>
        private float timer;
         
        /// <summary>
        /// Defines the waitSeconds.
        /// </summary>
        private float waitSeconds;

        /// <summary>
        /// Defines the WaitMinutes.
        /// </summary>
        public float WaitMinutes;

        /// <summary>
        /// The OnCheckLoacalData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public delegate Task OnCheckLoacalData();

        /// <summary>
        /// Defines the CheckLocalData.
        /// </summary>
        public event OnCheckLoacalData CheckLocalData;

        /// <summary>
        /// The Start.
        /// </summary>
        private void Start()
        {
            connectionService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<ConnectionService>();
            waitSeconds = 60 * WaitMinutes;
        }

        /// <summary>
        /// The Update.
        /// </summary>
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= waitSeconds)
            {
                timer = 0;
                if (connectionService.IsConnected)
                {
                    CheckLocalData();
                }
            }
        }
    }
}
