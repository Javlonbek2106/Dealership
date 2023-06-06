using Domain.Common;

public class Monitoring : BaseAuditableEntity
{
    public decimal Profit { get; set; }
    public int SoldCarCount { get; set; }
    public DateTime MonthYear { get; set; }
    // Other properties as needed

    // Relationships
    public Guid DealershipId { get; set; }
    public Dealership Dealership { get; set; }
}
