namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Configs
{
    using AppneuronUnity.Core.Libraries.LitJson;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="ChurnBlockerSingletonConfigs" />.
    /// </summary>
    public class ChurnBlockerSingletonConfigs
    {
        /// <summary>
        /// Defines the configList.
        /// </summary>
        private Dictionary<string, string> configList;

        /// <summary>
        /// Defines the instance.
        /// </summary>
        private static readonly ChurnBlockerSingletonConfigs instance = new ChurnBlockerSingletonConfigs();

        /// <summary>
        /// Initializes static members of the <see cref="ChurnBlockerSingletonConfigs"/> class.
        /// </summary>
        static ChurnBlockerSingletonConfigs()
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ChurnBlockerSingletonConfigs"/> class from being created.
        /// </summary>
        private ChurnBlockerSingletonConfigs()
        {
            ReadConfigList();
        }

        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static ChurnBlockerSingletonConfigs Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// The ReadConfigList.
        /// </summary>
        private void ReadConfigList()
        {
            configList = new Dictionary<string, string>();
            JsonReader reader = new JsonReader(File.ReadAllText(Application.streamingAssetsPath + "/ChurnBlocker.json"));
            while (reader.Read())
            {
                configList.Add(reader.Token.ToString(), reader.Value.ToString());
            }


        }

        /// <summary>
        /// The GetProjectID.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetProjectID()
        {
            return configList["ProjectID"];
        }

        /// <summary>
        /// The GetCustomerID.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetCustomerID()
        {
            return configList["CustomerID"];
        }
    }
}
