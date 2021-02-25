using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace word_pluralizer
{
    [Route("[controller]")]
    [ApiController]
    public class CollectorController : ControllerBase
    {
        private const string KafkaBootstrapUrl = "my-cluster-kafka-bootstrap.pluralizer.svc:9092";

        [HttpGet("/pluralize/{word}")]
        public string Pluralizer(string word)
        {
            return word + "s";
        }

        [HttpGet("/notify/{word}")]
        public string Notify(string word)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = KafkaBootstrapUrl,
                CompressionType = CompressionType.Snappy
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                producer.ProduceAsync("notifications", new Message<Null, string> { Value = word });
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }

			return word + "s";
        }
    }
}
