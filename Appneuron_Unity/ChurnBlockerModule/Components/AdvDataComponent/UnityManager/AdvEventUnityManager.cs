using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppneuronUnity.ChurnBlockerModule.Components.AdvDataComponent.UnityManager
{
    public class AdvEventUnityManager
    {

        public async Task CheckAdvFileAndSendData()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _advEventDal = kernel.Get<IAdvEventDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
          

            List<string> FolderNameList = ComponentsConfigService.GetSavedDataFilesNames(ComponentsConfigService
                                                                                         .SaveTypePath
                                                                                         .AdvEventDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _advEventDal.SelectAsync(ComponentsConfigService.AdvEventDataPath + fileName);

                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _advEventDal.DeleteAsync(ComponentsConfigService.AdvEventDataPath + fileName);
                }
            }
            }
        }



        public async Task SendAdvEventData(string Tag,
            string levelName,
            float GameSecond,
            string clientId)
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _advEventDal = kernel.Get<IAdvEventDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                var _cryptoServices = kernel.Get<ICryptoServices>();


                AdvEventDataModel advEventDataModel = new AdvEventDataModel
                {
                    ClientId = clientId,
                    ProjectID = ChurnBlockerSingletonConfigService.Instance.GetProjectID(),
                    CustomerID = ChurnBlockerSingletonConfigService.Instance.GetCustomerID(),
                    TrigersInlevelName = levelName,
                    AdvType = Tag,
                    InMinutes = GameSecond,
                    TrigerdTime = DateTime.Now
                };


                var result = await _kafkaMessageBroker.SendMessageAsync(advEventDataModel);
                if (result.Success)
                {
                    return;
                }

                string fileName = _cryptoServices.GenerateStringName(6);
                string filepath = ComponentsConfigService.AdvEventDataPath + fileName;

                await _advEventDal.InsertAsync(filepath, advEventDataModel);

            }
        }
    }
}
