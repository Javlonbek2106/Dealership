using Application.DTOs.Group;

namespace Application.DTO.Group
{
    public class CarCreateDTO : CarBaseDTO
    {
        public Guid DealershipId { get; set; }

    }
}
