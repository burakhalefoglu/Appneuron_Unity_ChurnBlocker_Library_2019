namespace AppneuronUnity.Core.UnityServices
{
    using AppneuronUnity.Core.CoreServices.RestClientServices.Abstract;
    using System.Reflection;
    using UnityEngine;
    using Appneuron.Zenject;

    /// <summary>
    /// Defines the <see cref="ConnectionService" />.
    /// </summary>
    public class ConnectionService : MonoBehaviour
    {
        /// <summary>
        /// Defines the isConnected.
        /// </summary>
        private bool isConnected;

        /// <summary>
        /// Defines the timer.
        /// </summary>
        private float timer;

        /// <summary>
        /// Defines the WaitTimeInSeconds.
        /// </summary>
        public float WaitTimeInSeconds;

        [Inject]
        private IRestClientServices _restClientServices;


        /// <summary>
        /// Gets a value indicating whether IsConnected.
        /// </summary>
        [HideInInspector]
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
        }

        /// <summary>
        /// The Update.
        /// </summary>
        private async void Update()
        {
            timer += Time.deltaTime;
            if (timer >= WaitTimeInSeconds)
            {
                timer = 0;
                var result = await _restClientServices.IsInternetConnectedAsync();
                isConnected = result.Success;
            }
        }
    }
}
