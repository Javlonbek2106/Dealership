using Application;
using Application.DTO.Group;
using Application.Interfaces.ModelInterface;
using Application.Models.PaginatedList;
using Application.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using ServiceCatalogUI.Controllers;
using ServiceCatalogUI.Filters;

namespace WebDealershipUI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CarController : ApiControllerBase<Car>
    {
        private readonly ICarRepository _carRepository;

        public CarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ResponseCore<PaginatedList<CarGetDTO>>>> Search(string searchQuery, int page = 1, int pageSize = 10)
        {
            IEnumerable<Car> cars = await _carRepository.GetAsync(x => x.Brand.ToLower().Contains(searchQuery.ToLower())
                                                        | x.Year.ToString().ToLower().Contains(searchQuery.ToLower())
                                                        | x.Model.ToLower().Contains(searchQuery.ToLower())
                                                        | x.Price.ToString().ToLower().Contains(searchQuery.ToLower()));

            IEnumerable<CarGetDTO> mappedCars = _mapper.Map<IEnumerable<CarGetDTO>>(cars);
            PaginatedList<CarGetDTO> car = PaginatedList<CarGetDTO>.CreateAsync(mappedCars, page, pageSize);
            return Ok(new ResponseCore<PaginatedList<CarGetDTO>>(car));
        }

        [HttpPost("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "CreateRole")]
        public async Task<ActionResult<ResponseCore<CarGetDTO>>> Create([FromBody] CarCreateDTO car)
        {
            Car mappedCar = _mapper.Map<Car>(car);
            var validationResult = _validator.Validate(mappedCar);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<object>(false, validationResult.Errors));
            }
            mappedCar = await _carRepository.CreateAsync(mappedCar);
            var res = _mapper.Map<CarGetDTO>(mappedCar);
            return Ok(new ResponseCore<object>(res));
        }



        [HttpGet("[action]")]
        //[Authorize(Roles = "GetAllRole")]
        public async Task<ActionResult<ResponseCore<PaginatedList<CarGetDTO>>>> GetAllCar(int page = 1, int pageSize = 10)
        {
            IEnumerable<Car> cars = await _carRepository.GetAsync(x => true);
            IEnumerable<CarGetDTO> mappedCars = _mapper.Map<IEnumerable<CarGetDTO>>(cars);
            PaginatedList<CarGetDTO> car = PaginatedList<CarGetDTO>.CreateAsync(mappedCars, page, pageSize);
            return Ok(new ResponseCore<PaginatedList<CarGetDTO>>(car));
        }



        [HttpPut("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "UpdateEmployee")]
        public async Task<ActionResult<ResponseCore<CarGetDTO>>> Update([FromBody] CarUpdateDTO car)
        {
            Car? mappedCar = _mapper.Map<Car>(car);
            var validationResult = _validator.Validate(mappedCar);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<Employee>(false, validationResult.Errors));
            }
            mappedCar = await _carRepository.UpdateAsync(mappedCar);
            if (mappedCar != null)
                return Ok(new ResponseCore<EmployeeGetDTO>(_mapper.Map<EmployeeGetDTO>(mappedCar)));
            return BadRequest(new ResponseCore<Employee>(false, car + " not found"));

        }

        [HttpDelete("[action]")]
        //[Authorize(Roles = "DeleteDealership")]
        public async Task<ActionResult<ResponseCore<bool>>> Delete(Guid id)
        {
            return await _carRepository.DeleteAsync(id) ?
                Ok(new ResponseCore<bool>(true))
              : BadRequest(new ResponseCore<bool>(false, "Delete failed!"));

        }
    }
}
