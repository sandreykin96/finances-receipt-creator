namespace FinancesReceiptCreator;

public class ItemsDto
{
    public string Name { get; set; }

    public long? Price { get; set; }

    public decimal? Quantity { get; set; }

    public long? Sum { get; set; }

    public int? Nds { get; set; }

    public long? NdsSum { get; set; }

    public int? ProductType { get; set; }

    public int? PaymentType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

}