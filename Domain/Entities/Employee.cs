using Domain.Common;
using Domain.Entities.IdentityEntities;

public class Employee : BaseAuditableEntity
{
    public string FullName { get; set; }
    public string Position { get; set; }
    public string Phone { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    // Other properties

    // Relationships
    public ICollection<SoldCar> SoldCars { get; set; }
    public ICollection<Role>? Roles { get; set; }
}
