using Confluent.Kafka;

namespace Ecommerce.Order.API.Core.Kafka.Connection
{
    public class KafkaConnectionProvider : IKafkaConnectionProvider
    {
        #region Properties
        private readonly IProducer<Null, string> _producerBuilder;
        private readonly IProducer<string, string> _producerWithKeyBuilder;
        private readonly IConsumer<Null, string> _consumerBuilder;
        #endregion

        #region Constructor
        public KafkaConnectionProvider(ProducerConfig producerConfig, ConsumerConfig consumerConfig)
        {
            _producerBuilder = new ProducerBuilder<Null, string>(producerConfig).Build();
            _producerWithKeyBuilder = new ProducerBuilder<string, string>(producerConfig).Build();
            _consumerBuilder = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        }
        #endregion

        #region GetProducer
        public IProducer<Null, string> GetProducer() => _producerBuilder;
        #endregion

        #region GetProducerWithKey
        public IProducer<string, string> GetProducerWithKey() => _producerWithKeyBuilder;
        #endregion

        #region GetConsumer
        public IConsumer<Null, string> GetConsumer() => _consumerBuilder;
        #endregion
    }
}
