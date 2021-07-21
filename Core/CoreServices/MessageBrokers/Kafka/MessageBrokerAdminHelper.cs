using Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Appneuron.Core.CoreServices.MessageBrockers.Kafka.Model;
using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Confluent.Kafka;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MessageBrokerAdminHelper
{
    private static IRestClientServices _restClientServices;

    public static async Task SetPartitionCountAsync()
    {
        using (var kernel = new StandardKernel())
        {

            kernel.Load(Assembly.GetExecutingAssembly());
            _restClientServices = kernel.Get<IRestClientServices>();

        }

        var result = await _restClientServices.IsInternetConnectedAsync();
        if (!result.Success)
            return;

        using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = ApacheKafkaConfigService.BootstrapServers }).Build())
        {

            var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(20));

            Debug.Log(meta.Topics);
            meta.Topics.ForEach(p =>
                {
                    PartitionsOfTopicDataModel.partitionModels.Add(new PartitionModel
                    {
                        PartitionCount = p.Partitions.Count(),
                        TopicName = p.Topic

                    });
                    Debug.Log(p);
                });


        }

    }
}

