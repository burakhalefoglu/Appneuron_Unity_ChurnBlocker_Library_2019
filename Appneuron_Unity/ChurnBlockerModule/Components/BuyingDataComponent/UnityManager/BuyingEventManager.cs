using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AppneuronUnity.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager
{
    public class BuyingEventManager
    {
        public async Task CheckAdvFileAndSendData()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _buyingEventDal = kernel.Get<IBuyingEventDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();

                List<string> FolderList = ComponentsConfigService.GetSavedDataFilesNames(ComponentsConfigService
                                                                                          .SaveTypePath
                                                                                          .BuyingEventDataModel);

                foreach (var fileName in FolderList)
                {
                    var dataModel = await _buyingEventDal.SelectAsync(ComponentsConfigService.BuyingEventDataPath + fileName);

                    var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                    if (result.Success)
                    {
                        await _buyingEventDal.DeleteAsync(ComponentsConfigService.AdvEventDataPath + fileName);
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
                var _buyingEventDal = kernel.Get<IBuyingEventDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                var _cryptoServices = kernel.Get<ICryptoServices>();



                BuyingEventDataModel dataModel = new BuyingEventDataModel
                {

                    ClientId = clientId,
                    ProjectID = ChurnBlockerSingletonConfigService.Instance.GetProjectID(),
                    CustomerID = ChurnBlockerSingletonConfigService.Instance.GetCustomerID(),
                    TrigersInlevelName = levelName,
                    ProductType = Tag,
                    InWhatMinutes = GameSecond,
                    TrigerdTime = DateTime.Now

                };


                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    return;
                }
                string fileName = _cryptoServices.GenerateStringName(6);
                string filepath = ComponentsConfigService.AdvEventDataPath + fileName;

                await _buyingEventDal.InsertAsync(filepath, dataModel);
            }
        }
    }
}
