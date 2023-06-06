namespace Application.DTOs
{
    public class EmployeeCreateDTO : EmployeeBaseDTO
    {
       
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public List<Guid>? RolesId { get; set; }

    }
}
