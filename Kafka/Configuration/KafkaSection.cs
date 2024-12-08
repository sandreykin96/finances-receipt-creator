namespace Kafka.Configuration;

public sealed class KafkaSection
{
	internal const string SectionName = "Kafka";

	public string Uri { get; set; } = "localhost:9092";

	public string OutputTopic { get; set; } = "esf";
    
	public string GroupId { get; set; } = "123";

	public double LingerMs { get; set; } = 5;

	public int BatchSize { get; set; } = 1000000;

	public int BatchNumMessages { get; set; } = 10000;

	public int MessageSendMaxRetries { get; set; } = 3;

	public int FlushTimeout { get; set; } = 5;

	public int MessageMaxBytes { get; set; } = 1000000;

	public int MessageTimeout { get; set; } = 10000;
}