namespace AppneuronUnity.Core.UnityServices
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="DontDestroyOnLoadService" />.
    /// </summary>
    public class DontDestroyOnLoadService : MonoBehaviour
    {
        /// <summary>
        /// The Start.
        /// </summary>
        private void Start()
        {
            DontDestroyOnLoad();
        }

        /// <summary>
        /// The DontDestroyOnLoad.
        /// </summary>
        private void DontDestroyOnLoad()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Appneuron");

            if (objs.Length > 1)
            {
                for (int i = 1; i < objs.Length; i++)
                {
                    Destroy(objs[i]);
                }
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}
