using Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.CoreServices.ResultService;
using Confluent.Kafka;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka
{
    public class KafkaMessageHelper : IKafkaMessageBroker
    {
        private IRestClientServices _restClientServices;

        public Task<IDataResult<T>> getMessageAsync<T>() where T : class, new()
        {

            throw new NotImplementedException();
        }

        public async Task<IResult> SendMessageAsync<T>(T messageModel) where T :
         class, new()
        {

            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                _restClientServices = kernel.Get<IRestClientServices>();

            }

            var result = await _restClientServices.IsInternetConnectedAsync();
            if (!result.Success)
                return new ErrorResult();

            if (PartitionsOfTopicDataModel.partitionModels.Count==0)
            {
                await MessageBrokerAdminHelper.SetPartitionCountAsync();
            }

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = ApacheKafkaConfigService.BootstrapServers,
                Acks = Acks.All
            };

            var message = JsonConvert.SerializeObject(messageModel);
            var topicName = typeof(T).Name;
            using (var p = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                try
                {
                    var partitionModel = PartitionsOfTopicDataModel.partitionModels.Find(t => t.TopicName == topicName);

                    await p.ProduceAsync(new TopicPartition(topicName,
                        new Partition(new System.Random().Next(0, partitionModel.PartitionCount)))
                    , new Message<Null, string>
                    {
                        Value = message

                    });
                    return new SuccessResult();

                }

                catch (ProduceException<Null, string> e)
                {
                    return new ErrorResult();
                }
            }
        }
    }
}
