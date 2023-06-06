namespace Application.DTOs.Teacher
{
    public class SoldCarCreateDTO : SoldCarBaseDTO
    {
        public Guid CarId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
