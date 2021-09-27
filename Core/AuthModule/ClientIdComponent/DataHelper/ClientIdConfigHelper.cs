namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.Helper
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.Helper;

    /// <summary>
    /// Defines the <see cref="ClientIdConfigServices" />.
    /// </summary>
    internal static class ClientIdConfigServices
    {
        /// <summary>
        /// Defines the IdPath.
        /// </summary>
        public enum IdPath
        {
            /// <summary>
            /// Defines the id.
            /// </summary>
            id
        }

        /// <summary>
        /// Defines the dataPath.
        /// </summary>
        internal static readonly string dataPath = Application.persistentDataPath;

        /// <summary>
        /// Defines the GeneralData.
        /// </summary>
        public static readonly Dictionary<IdPath, string> GeneralData = new Dictionary<IdPath, string>
        {
            {IdPath.id, dataPath + "/ChurnBlocker/GeneralData/IdData/"}
        };

        /// <summary>
        /// The CreateFileDirectories.
        /// </summary>
        public static void CreateFileDirectories()
        {
            foreach (KeyValuePair<IdPath, string> directory in GeneralData)
            {
                Directory.CreateDirectory(directory.Value);
            }
        }

        /// <summary>
        /// Defines the CustomerIdPath.
        /// </summary>
        public static readonly string CustomerIdPath = GeneralData[IdPath.id];
    }
}
