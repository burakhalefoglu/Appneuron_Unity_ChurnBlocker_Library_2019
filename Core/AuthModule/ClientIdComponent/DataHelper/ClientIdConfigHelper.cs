namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.DataHelper
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    internal static class ClientIdConfigServices
    {

        public enum IdPath
        {
            id
        }

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
