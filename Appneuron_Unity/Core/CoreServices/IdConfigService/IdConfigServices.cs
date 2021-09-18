using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.Core.CoreServices.IdConfigService
{
    public static class IdConfigServices
    {

        public enum IdPath
        {
            id

        }
        static readonly string dataPath = Application.persistentDataPath;

        public static readonly Dictionary<IdPath, string> GeneralData = new Dictionary<IdPath, string>
        {
            {IdPath.id, dataPath + "/ChurnBlocker/GeneralData/IdData/"}
        };

        public static void CreateFileDirectories()
        {
            foreach (KeyValuePair<IdPath, string> directory in GeneralData)
            {
                Directory.CreateDirectory(directory.Value);
            }
        }

        public static readonly string CustomerIdPath = GeneralData[IdPath.id];


    }
}
