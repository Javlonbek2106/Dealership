using Application.DTOs.Lesson;
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
    public class DealershipController : ApiControllerBase<Dealership>
    {
        private readonly IDealershipRepository dealershipRepository;

        public DealershipController(IDealershipRepository dealershipRepository)
        {
            this.dealershipRepository = dealershipRepository;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ResponseCore<PaginatedList<DealershipGetDTO>>>> Search(string searchQuery, int page = 1, int pageSize = 10)
        {
            IEnumerable<Dealership> dealerships = await dealershipRepository.GetAsync(x => x.Address.Contains(searchQuery)
                                                        | x.Phone.ToString().Contains(searchQuery)
                                                        | x.Name.Contains(searchQuery));
            IEnumerable<DealershipGetDTO> mappeddealerships = _mapper.Map<IEnumerable<DealershipGetDTO>>(dealerships);

            PaginatedList<DealershipGetDTO> dealership = PaginatedList<DealershipGetDTO>.CreateAsync(mappeddealerships, page, pageSize);
            return Ok(new ResponseCore<PaginatedList<DealershipGetDTO>>(dealership));
        }
        [HttpPost("[action]")]
        //[ActionModelValidation]
        //[Authorize(Roles = "Createdealership")]
        public async Task<ActionResult<ResponseCore<DealershipGetDTO>>> Create([FromBody] DealershipCreateDTO dealership)
        {
            Dealership mappedDealership = _mapper.Map<Dealership>(dealership);
            var validationResult = _validator.Validate(mappedDealership);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<object>(false, validationResult.Errors));
            }

            mappedDealership = await dealershipRepository.CreateAsync(mappedDealership);
            var res = _mapper.Map<DealershipGetDTO>(mappedDealership);
            return Ok(new ResponseCore<object>(res));
        }

        [HttpGet("[action]")]
        //[Authorize(Roles = "GetAllDealership")]
        public async Task<ActionResult<ResponseCore<IEnumerable<DealershipGetDTO>>>> GetAll(int page = 1, int pageSize = 10)
        {
            IEnumerable<Dealership> dealerships = await dealershipRepository.GetAsync(x => true);
            IEnumerable<DealershipGetDTO> mappedDealerships = _mapper.Map<IEnumerable<DealershipGetDTO>>(dealerships);
            PaginatedList<DealershipGetDTO> dealership = PaginatedList<DealershipGetDTO>.CreateAsync(mappedDealerships, page, pageSize);
            return Ok(new ResponseCore<PaginatedList<DealershipGetDTO>>(dealership));
        }

        [HttpGet("[action]")]
        //[Authorize(Roles = "GetByIdDealership")]
        public async Task<ActionResult<ResponseCore<DealershipGetDTO>>> GetById(Guid id)
        {
            Dealership? obj = await dealershipRepository.GetByIdAsync(id);
            if (obj == null)
            {
                return NotFound(new ResponseCore<Dealership?>(false, id + " not found!"));
            }
            DealershipGetDTO mappedDealership = _mapper.Map<DealershipGetDTO>(obj);
            return Ok(new ResponseCore<DealershipGetDTO?>(mappedDealership));
        }

        [HttpPut("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "UpdateEmployee")]
        public async Task<ActionResult<ResponseCore<DealershipGetDTO>>> Update([FromBody] DealershipUpdateDTO employee)
        {
            Dealership? mappedDealership = _mapper.Map<Dealership>(employee);
            var validationResult = _validator.Validate(mappedDealership);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<Employee>(false, validationResult.Errors));
            }

            mappedDealership = await dealershipRepository.UpdateAsync(mappedDealership);
            if (mappedDealership != null)
                return Ok(new ResponseCore<DealershipGetDTO>(_mapper.Map<DealershipGetDTO>(mappedDealership)));
            return BadRequest(new ResponseCore<Dealership>(false, employee + " not found"));

        }

        [HttpDelete("[action]")]
        //[Authorize(Roles = "DeleteDealership")]
        public async Task<ActionResult<ResponseCore<bool>>> Delete(Guid id)
        {
            return await dealershipRepository.DeleteAsync(id) ?
                Ok(new ResponseCore<bool>(true))
              : BadRequest(new ResponseCore<bool>(false, "Delete failed!"));

        }
    }
}
