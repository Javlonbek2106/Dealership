using Domain.Common;

public class Car : BaseAuditableEntity
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public string Url { get; set; }
    // Other properties

    // Relationships
    public Guid DealershipId { get; set; }
    public Dealership Dealership { get; set; }
    public ICollection<SoldCar> SoldCars { get; set; }
}
