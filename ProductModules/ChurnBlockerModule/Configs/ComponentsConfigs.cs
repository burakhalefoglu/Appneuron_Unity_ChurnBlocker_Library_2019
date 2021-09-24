namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Configs
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="ComponentsConfigs" />.
    /// </summary>
    public class ComponentsConfigs
    {
        /// <summary>
        /// Defines the SaveTypePath.
        /// </summary>
        public enum SaveTypePath
        {
            /// <summary>
            /// Defines the AdvEventDataModel.
            /// </summary>
            AdvEventDataModel,
            /// <summary>
            /// Defines the CustomerIdModel.
            /// </summary>
            CustomerIdModel,
            /// <summary>
            /// Defines the SuccessSaveInfo.
            /// </summary>
            SuccessSaveInfo,
            /// <summary>
            /// Defines the LevelBaseDieDataModel.
            /// </summary>
            LevelBaseDieDataModel,
            /// <summary>
            /// Defines the EveryLoginLevelDatasModel.
            /// </summary>
            EveryLoginLevelDatasModel,
            /// <summary>
            /// Defines the LevelsAvarageDatasModel.
            /// </summary>
            LevelsAvarageDatasModel,
            /// <summary>
            /// Defines the DailySessionDataModel.
            /// </summary>
            DailySessionDataModel,
            /// <summary>
            /// Defines the GameSessionEveryLoginDataModel.
            /// </summary>
            GameSessionEveryLoginDataModel,
            /// <summary>
            /// Defines the LevelBaseSessionDataModel.
            /// </summary>
            LevelBaseSessionDataModel,
            /// <summary>
            /// Defines the GeneralDataModel.
            /// </summary>
            GeneralDataModel,
            /// <summary>
            /// Defines the BuyingEventDataModel.
            /// </summary>
            BuyingEventDataModel,
            /// <summary>
            /// Defines the TokenDataModel.
            /// </summary>
            TokenDataModel,
            /// <summary>
            /// Defines the DifficultyModel.
            /// </summary>
            DifficultyModel,
            /// <summary>
            /// Defines the InventoryDataModel.
            /// </summary>
            InventoryDataModel,
            /// <summary>
            /// Defines the SwipeDataModel.
            /// </summary>
            SwipeDataModel,
            /// <summary>
            /// Defines the ClickDataModel.
            /// </summary>
            ClickDataModel
        }

        /// <summary>
        /// Defines the dataPath.
        /// </summary>
        private static readonly string dataPath = Application.persistentDataPath;

        /// <summary>
        /// Defines the ComponentsData.
        /// </summary>
        public readonly static Dictionary<SaveTypePath, string> ComponentsData = new Dictionary<SaveTypePath, string>
        {
            {SaveTypePath.AdvEventDataModel, dataPath + "/ChurnBlocker/ComponentsData/AdvEventDataModel/"},
            {SaveTypePath.CustomerIdModel, dataPath + "/ChurnBlocker/ComponentsData/CustomerIdModel/"},
            {SaveTypePath.SuccessSaveInfo, dataPath + "/ChurnBlocker/ComponentsData/SuccessSaveInfo/"},
            {SaveTypePath.LevelsAvarageDatasModel, dataPath + "/ChurnBlocker/ComponentsData/LevelsAvarageDatasModel/"},
            {SaveTypePath.LevelBaseDieDataModel, dataPath + "/ChurnBlocker/ComponentsData/LevelBaseDieDataModel/"},
            {SaveTypePath.EveryLoginLevelDatasModel, dataPath + "/ChurnBlocker/ComponentsData/EveryLoginLevelDatasModel/"},
            {SaveTypePath.DailySessionDataModel, dataPath + "/ChurnBlocker/ComponentsData/DailySessionDataModel/"},
            {SaveTypePath.GameSessionEveryLoginDataModel, dataPath + "/ChurnBlocker/ComponentsData/GameSessionEveryLoginDataModel/"},
            {SaveTypePath.LevelBaseSessionDataModel, dataPath + "/ChurnBlocker/ComponentsData/LevelBaseSessionDataModel/"},
            {SaveTypePath.GeneralDataModel, dataPath + "/ChurnBlocker/ComponentsData/GeneralDataModel/"},
            {SaveTypePath.BuyingEventDataModel, dataPath + "/ChurnBlocker/ComponentsData/BuyingEventDataModel/" },
            {SaveTypePath.TokenDataModel, dataPath + "/ChurnBlocker/TokenDataModel/"},
            {SaveTypePath.DifficultyModel, dataPath + "/ChurnBlocker/DifficultyModel/"},
            {SaveTypePath.InventoryDataModel, dataPath + "/ChurnBlocker/DifficultyModel/InventoryDataModel"},
            {SaveTypePath.SwipeDataModel, dataPath + "/ChurnBlocker/DifficultyModel/SwipeDataModel"},
            {SaveTypePath.ClickDataModel, dataPath + "/ChurnBlocker/DifficultyModel/ClickDataModel"}
        };

        /// <summary>
        /// Defines the AdvEventDataPath.
        /// </summary>
        public readonly static string AdvEventDataPath = ComponentsData[SaveTypePath.AdvEventDataModel];

        /// <summary>
        /// Defines the CustomerIdPath.
        /// </summary>
        public readonly static string CustomerIdPath = ComponentsData[SaveTypePath.CustomerIdModel];

        /// <summary>
        /// Defines the SuccessSaveInfoPath.
        /// </summary>
        public readonly static string SuccessSaveInfoPath = ComponentsData[SaveTypePath.SuccessSaveInfo];

        /// <summary>
        /// Defines the LevelsAvarageDatasPath.
        /// </summary>
        public readonly static string LevelsAvarageDatasPath = ComponentsData[SaveTypePath.LevelsAvarageDatasModel];

        /// <summary>
        /// Defines the LevelBaseDieDataPath.
        /// </summary>
        public readonly static string LevelBaseDieDataPath = ComponentsData[SaveTypePath.LevelBaseDieDataModel];

        /// <summary>
        /// Defines the EveryLoginLevelDatasPath.
        /// </summary>
        public readonly static string EveryLoginLevelDatasPath = ComponentsData[SaveTypePath.EveryLoginLevelDatasModel];

        /// <summary>
        /// Defines the DailySessionDataPath.
        /// </summary>
        public readonly static string DailySessionDataPath = ComponentsData[SaveTypePath.DailySessionDataModel];

        /// <summary>
        /// Defines the GameSessionEveryLoginDataPath.
        /// </summary>
        public readonly static string GameSessionEveryLoginDataPath = ComponentsData[SaveTypePath.GameSessionEveryLoginDataModel];

        /// <summary>
        /// Defines the LevelBaseSessionDataPath.
        /// </summary>
        public readonly static string LevelBaseSessionDataPath = ComponentsData[SaveTypePath.LevelBaseSessionDataModel];

        /// <summary>
        /// Defines the GeneralDataPath.
        /// </summary>
        public readonly static string GeneralDataPath = ComponentsData[SaveTypePath.GeneralDataModel];

        /// <summary>
        /// Defines the BuyingEventDataPath.
        /// </summary>
        public readonly static string BuyingEventDataPath = ComponentsData[SaveTypePath.BuyingEventDataModel];

        /// <summary>
        /// Defines the TokenDataModel.
        /// </summary>
        public readonly static string TokenDataModel = ComponentsData[SaveTypePath.TokenDataModel];

        /// <summary>
        /// Defines the DifficultyModel.
        /// </summary>
        public readonly static string DifficultyModel = ComponentsData[SaveTypePath.DifficultyModel];

        /// <summary>
        /// Defines the InventoryDataPath.
        /// </summary>
        public readonly static string InventoryDataPath = ComponentsData[SaveTypePath.InventoryDataModel];

        /// <summary>
        /// Defines the SwipeDataModelPath.
        /// </summary>
        public readonly static string SwipeDataModelPath = ComponentsData[SaveTypePath.SwipeDataModel];

        /// <summary>
        /// Defines the ClickDataModelPath.
        /// </summary>
        public readonly static string ClickDataModelPath = ComponentsData[SaveTypePath.ClickDataModel];

        /// <summary>
        /// The CreateFileLocalDataDirectories.
        /// </summary>
        public static void CreateFileLocalDataDirectories()
        {
            foreach (KeyValuePair<SaveTypePath, string> directory in ComponentsData)
            {
                Directory.CreateDirectory(directory.Value);
            }
        }

        /// <summary>
        /// The GetSavedDataFilesNames.
        /// </summary>
        /// <param name="fileType">The fileType<see cref="SaveTypePath"/>.</param>
        /// <returns>The <see cref="List{string}"/>.</returns>
        public static List<string> GetSavedDataFilesNames(SaveTypePath fileType)
        {
            DirectoryInfo dir = new DirectoryInfo(ComponentsData[fileType]);
            FileInfo[] info = dir.GetFiles("*" + ".data");
            List<string> fileNames = new List<string>();
            foreach (FileInfo f in info)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(f.FullName));
            }
            return fileNames;
        }
    }
}
