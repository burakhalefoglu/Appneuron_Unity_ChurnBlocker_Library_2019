using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Appneuron.Services
{
   public class ComponentsConfigService
    {
        public enum SaveTypePath
        {
            AdvEventDataModel,
            CustomerIdModel,
            SuccessSaveInfo,
            LevelBaseDieDataModel,
            EveryLoginLevelDatasModel,
            LevelsAvarageDatasModel,
            DailySessionDataModel,
            GameSessionEveryLoginDataModel,
            LevelBaseSessionDataModel,
            GeneralDataModel,
            BuyingEventDataModel
        }

        private static readonly string dataPath = Application.persistentDataPath;

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
            {SaveTypePath.BuyingEventDataModel, dataPath + "/ChurnBlocker/ComponentsData/BuyingEventDataModel/"}
        };

        public readonly static string AdvEventDataPath = ComponentsData[SaveTypePath.AdvEventDataModel];
        public readonly static string CustomerIdPath = ComponentsData[SaveTypePath.CustomerIdModel];
        public readonly static string SuccessSaveInfoPath = ComponentsData[SaveTypePath.SuccessSaveInfo];
        public readonly static string LevelsAvarageDatasPath = ComponentsData[SaveTypePath.LevelsAvarageDatasModel];
        public readonly static string LevelBaseDieDataPath = ComponentsData[SaveTypePath.LevelBaseDieDataModel];
        public readonly static string EveryLoginLevelDatasPath = ComponentsData[SaveTypePath.EveryLoginLevelDatasModel];
        public readonly static string DailySessionDataPath = ComponentsData[SaveTypePath.DailySessionDataModel];
        public readonly static string GameSessionEveryLoginDataPath = ComponentsData[SaveTypePath.GameSessionEveryLoginDataModel];
        public readonly static string LevelBaseSessionDataPath = ComponentsData[SaveTypePath.LevelBaseSessionDataModel];
        public readonly static string GeneralDataPath = ComponentsData[SaveTypePath.GeneralDataModel];
        public readonly static string BuyingEventDataPath = ComponentsData[SaveTypePath.BuyingEventDataModel];





        public static void CreateFileVisualDataDirectories()
        {
            foreach (KeyValuePair<SaveTypePath, string> directory in ComponentsData)
            {
                Directory.CreateDirectory(directory.Value);
            }
        }

        public static List<string> GetVisualDataFilesName(SaveTypePath fileType)
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
