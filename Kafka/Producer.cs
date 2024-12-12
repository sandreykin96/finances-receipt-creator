using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lib.Kafka;

public class Producer : IProducer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly Lazy<IProducer<Null, byte[]>> _producerLazy;
    private readonly ILogger<Producer> _logger;

    public Producer(IConfiguration config, ILogger<Producer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _producerLazy = new Lazy<IProducer<Null, byte[]>>(CreateProducer);

        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
    }

    public async Task SendAsync<T>(IEnumerable<T> messages, string topic = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var message in messages)
            {
                await _producerLazy.Value.ProduceAsync(topic, CreateMessage(message),
                    cancellationToken);
            }

            _producerLazy.Value.Flush(TimeSpan.FromSeconds(30));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Produce error");
            throw;
        }
    }

    private Message<Null, byte[]> CreateMessage<T>(T message) =>
        new() { Value = JsonSerializer.SerializeToUtf8Bytes(message, _jsonSerializerOptions) };

    private IProducer<Null, byte[]> CreateProducer()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            LingerMs = 5,
            BatchSize = 10000,
            BatchNumMessages = 1000,
            Acks = Acks.Leader
        };

        return new ProducerBuilder<Null, byte[]>(producerConfig)
            .SetLogHandler((_, message) =>
                _logger.LogDebug($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
            .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
            .Build();
    }
}