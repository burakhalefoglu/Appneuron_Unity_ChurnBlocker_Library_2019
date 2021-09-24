namespace AppneuronUnity.ProductModules.ChurnBlockerModule
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="ChurnBlockerModule" />.
    /// </summary>
    public class ChurnBlockerModule : MonoBehaviour
    {
        /// <summary>
        /// The Awake.
        /// </summary>
        private void Awake()
        {
            ComponentsConfigs.CreateFileLocalDataDirectories();
        }
    }
}
