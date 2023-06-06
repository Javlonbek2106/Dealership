using Application.DTOs;
using Domain.Entities.IdentityEntities;

namespace Application
{
    public class EmployeeGetDTO : EmployeeBaseDTO
    {
        public Guid Id { get; set; }
        public string[]? Roles { get; set; }

    }
}
