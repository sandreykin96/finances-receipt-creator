namespace FinancesReceiptCreator;

/// <summary>
/// ���
/// </summary>
public class ReceiptDto
{
    public IEnumerable<ItemsDto> Items { get; set; }

    public long? Id { get; set; }

    public string User { get; set; }
}