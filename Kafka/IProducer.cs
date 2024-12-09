namespace Lib.Kafka;

public interface IProducer
{
    Task SendAsync<T>(IEnumerable<T> messages, string topic = default, CancellationToken cancellationToken = default);
}