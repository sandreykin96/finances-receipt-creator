namespace FinancesReceiptCreator;

/// <summary>
/// Чек
/// </summary>
public class ReceiptDto
{
    public IEnumerable<ItemsDto> Items { get; set; }

    public long? Id { get; set; }

    public int User { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}