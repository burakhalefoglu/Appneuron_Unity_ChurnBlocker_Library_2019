using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Appneuron.Services
{
    public class ChurnBlockerSingletonConfigService
    {

        private List<string> configList;

        private static readonly ChurnBlockerSingletonConfigService instance = new ChurnBlockerSingletonConfigService();

        static ChurnBlockerSingletonConfigService()
        {
        }

        private ChurnBlockerSingletonConfigService()
        {
            ReadConfigList();
        }

        public static ChurnBlockerSingletonConfigService Instance
        {
            get
            {
                return instance;
            }
        }


        private void ReadConfigList()
        {
            configList = new List<string>();
            JObject appneuronJsonFile = JObject.Parse(File.ReadAllText(Application.streamingAssetsPath + "/ChurnBlocker.json"));
            foreach (JProperty property in appneuronJsonFile.Properties())
            {
                configList.Add(property.Value.ToString());
            }
        }

        public string GetProjectID()
        {
            return configList[0];
        }

        public string GetCustomerID()
        {
            return configList[1];
        }
    }

}
