using Domain.Common;

public class Dealership : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string WorkingHours { get; set; }
    // Other properties

    // Relationships
    public ICollection<Car> Cars { get; set; }
    public ICollection<SoldCar> SoldCars { get; set; }
    public ICollection<Monitoring> Monitorings { get; set; }
}
