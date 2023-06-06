using Domain.Common;

public class SoldCar : BaseAuditableEntity
{
    public DateTime SoldDate { get; set; }
    public decimal SoldPrice { get; set; }
    // Other properties

    // Relationships
    public Guid CarId { get; set; }
    public Car Car { get; set; }
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
