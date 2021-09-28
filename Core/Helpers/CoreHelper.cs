namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Configs
{
    using AppneuronUnity.Core.Libraries.LitJson;
    using AppneuronUnity.Core.Models.Concrete;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    internal class CoreHelper
    {
        ProjectInfoModel projectInfoModel;
        public CoreHelper()
        {
            string jsonfile = File.ReadAllText(Application.streamingAssetsPath + "/ChurnBlocker.json");
            var projectInfo = JsonMapper.ToObject<ProjectInfoModel>(jsonfile);
            projectInfoModel = projectInfo;
        }
         
        public List<string> GetSavedDataFilesNames<T>()
        {
            var directoryName = Application.persistentDataPath + $"/Appneuron/{typeof(T).Name}/";
            if (!File.Exists(directoryName))
            {

                return new List<string>();
            }
            DirectoryInfo dir = new DirectoryInfo(directoryName);
            FileInfo[] info = dir.GetFiles("*" + ".data");
            List<string> fileNames = new List<string>();
            foreach (FileInfo f in info)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(f.FullName));
            }
            return fileNames;
        }

        public ProjectInfoModel GetProjectInfo()
        {
                return projectInfoModel;
        }
    }
}
