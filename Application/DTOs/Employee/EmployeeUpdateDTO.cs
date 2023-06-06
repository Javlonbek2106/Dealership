namespace Application.DTOs
{
    public class EmployeeUpdateDTO : EmployeeBaseDTO
    {
        public Guid Id { get; set; }
        public List<Guid>? RolesId { get; set; }

    }
}
