using Confluent.Kafka;

namespace Ecommerce.Order.API.Core.Kafka.Connection
{
    public interface IKafkaConnectionProvider
    {
        IProducer<Null, string> GetProducer();

        IConsumer<Null, string> GetConsumer();
    }
}
