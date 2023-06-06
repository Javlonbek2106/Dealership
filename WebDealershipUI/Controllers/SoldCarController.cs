using Application.DTO.Group;
using Application.ResponseModel;
using Application;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ServiceCatalogUI.Controllers;
using ServiceCatalogUI.Filters;
using Application.DTOs.Teacher;
using Application.Interfaces.ModelInterface;
using Microsoft.AspNetCore.Authorization;

namespace WebDealershipUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SoldSoldCarController : ApiControllerBase<SoldCar>
    {
        private readonly ISoldCarRepository soldCarRepository;

        public SoldSoldCarController(ISoldCarRepository soldCarRepository)
        {
            this.soldCarRepository = soldCarRepository;
        }

        [HttpPost("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "CreateRole")]
        public async Task<ActionResult<ResponseCore<SoldCarGetDTO>>> Create([FromBody] SoldCarCreateDTO car)
        {
            SoldCar mappedSoldCar = _mapper.Map<SoldCar>(car);
            var validationResult = _validator.Validate(mappedSoldCar);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<object>(false, validationResult.Errors));
            }

            mappedSoldCar = await soldCarRepository.CreateAsync(mappedSoldCar);
            var res = _mapper.Map<SoldCarGetDTO>(mappedSoldCar);
            return Ok(new ResponseCore<object>(res));
        }


        [HttpGet("[action]")]
        //[Authorize(Roles = "GetAllRole")]
        public async Task<ActionResult<ResponseCore<IEnumerable<SoldCarGetDTO>>>> GetAllSoldCar()
        {
            IEnumerable<SoldCar> cars = await soldCarRepository.GetAsync(x => true);
            IEnumerable<SoldCarGetDTO> mappedRoles = _mapper.Map<IEnumerable<SoldCarGetDTO>>(cars);

            return Ok(new ResponseCore<IEnumerable<SoldCarGetDTO>>(mappedRoles));
        }

        [HttpPut("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "UpdateEmployee")]
        public async Task<ActionResult<ResponseCore<SoldCarGetDTO>>> Update([FromBody] SoldCarUpdateDTO car)
        {
            SoldCar? mappedSoldCar = _mapper.Map<SoldCar>(car);
            var validationResult = _validator.Validate(mappedSoldCar);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<Employee>(false, validationResult.Errors));
            }
            mappedSoldCar = await soldCarRepository.UpdateAsync(mappedSoldCar);
            if (mappedSoldCar != null)
                return Ok(new ResponseCore<EmployeeGetDTO>(_mapper.Map<EmployeeGetDTO>(mappedSoldCar)));
            return BadRequest(new ResponseCore<Employee>(false, car + " not found"));

        }

        [HttpDelete("[action]")]
        //[Authorize(Roles = "DeleteDealership")]
        public async Task<ActionResult<ResponseCore<bool>>> Delete(Guid id)
        {
            return await soldCarRepository.DeleteAsync(id) ?
                Ok(new ResponseCore<bool>(true))
              : BadRequest(new ResponseCore<bool>(false, "Delete failed!"));

        }
    }
}
