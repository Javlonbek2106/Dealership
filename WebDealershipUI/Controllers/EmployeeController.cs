using Application;
using Application.DTOs;
using Application.Interfaces.ModelInterface;
using Application.Interfaces.ModelInterface.Login;
using Application.ResponseModel;
using Domain.Entities.IdentityEntities;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceCatalogUI.Filters;

namespace ServiceCatalogUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeController : ApiControllerBase<Employee>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoleRepository _roleRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IRoleRepository roleRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _roleRepository = roleRepository;
            _roleRepository = roleRepository;
        }
        [HttpPost("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "CreateEmployee")]
        public async Task<ActionResult<ResponseCore<EmployeeGetDTO>>> Create([FromBody] EmployeeCreateDTO employee)
        {
            Employee mappedEmployee = _mapper.Map<Employee>(employee);
            var validationResult = _validator.Validate(mappedEmployee);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<object>(false, validationResult.Errors));
            }
            mappedEmployee.Roles = new List<Role>();
            foreach (Guid item in employee.RolesId)
            {
                Role? role = await _roleRepository.GetByIdAsync(item);
                if (role != null)
                    mappedEmployee.Roles.Add(role);
                else return BadRequest(new ResponseCore<string>(false, item + " Id not found"));
            }
            mappedEmployee.Password=mappedEmployee.Password.ComputeSha256Hash();
            mappedEmployee = await _employeeRepository.CreateAsync(mappedEmployee);
            var res = _mapper.Map<EmployeeGetDTO>(mappedEmployee);
            return Ok(new ResponseCore<EmployeeGetDTO>(res));

        }

        [HttpGet("[action]")]
        //[Authorize(Roles = "GetAllEmployee")]
        public async Task<ActionResult<ResponseCore<IEnumerable<EmployeeGetDTO>>>> GetAll(string searchQuery = null)
        {
            await Console.Out.WriteLineAsync("Employee");
            IEnumerable<Employee> employees;

            if (searchQuery != null)
            {
                employees = await _employeeRepository.GetAsync(x =>
                    string.IsNullOrEmpty(searchQuery) || x.FullName.Contains(searchQuery), nameof(Employee.Roles));
            }
            else
            {
                employees = await _employeeRepository.GetAsync(x => true, nameof(Employee.Roles));
            }

            IEnumerable<EmployeeGetDTO> mappedEmployees = _mapper.Map<IEnumerable<EmployeeGetDTO>>(employees);

            return Ok(new ResponseCore<IEnumerable<EmployeeGetDTO>>(mappedEmployees));
        }


        [HttpGet("[action]")]
        //[Authorize(Roles = "GetByIdEmployee")]
        public async Task<ActionResult<ResponseCore<EmployeeGetDTO>>> GetById(Guid id)
        {
            Employee? obj = await _employeeRepository.GetByIdAsync(id);
            if (obj == null)
            {
                return NotFound(new ResponseCore<Employee?>(false, id + " not found!"));
            }
            EmployeeGetDTO mappedEmployee = _mapper.Map<EmployeeGetDTO>(obj);
            return Ok(new ResponseCore<EmployeeGetDTO?>(mappedEmployee));
        }

        [HttpPut("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "UpdateEmployee")]
        public async Task<ActionResult<ResponseCore<EmployeeGetDTO>>> Update([FromBody] EmployeeUpdateDTO employee)
        {
            Employee? mappedEmployee = _mapper.Map<Employee>(employee);
            var validationResult = _validator.Validate(mappedEmployee);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<Employee>(false, validationResult.Errors));
            }
            mappedEmployee.Roles = new List<Role>();
            foreach (var roleId in employee.RolesId)
            {
                Role? role = await _roleRepository.GetByIdAsync(roleId);
                if (role != null)
                    mappedEmployee.Roles.Add(role);
                else return BadRequest(new ResponseCore<Employee>(false, roleId + " Id not found"));
            }
            mappedEmployee = await _employeeRepository.UpdateAsync(mappedEmployee);
            if (mappedEmployee != null)
                return Ok(new ResponseCore<EmployeeGetDTO>(_mapper.Map<EmployeeGetDTO>(mappedEmployee)));
            return BadRequest(new ResponseCore<Employee>(false, employee + " not found"));

        }

        

        [HttpDelete("[action]")]
        //[Authorize(Roles = "DeleteEmployee")]
        public async Task<ActionResult<ResponseCore<bool>>> Delete(Guid id)
        {
            return await _employeeRepository.DeleteAsync(id) ?
                Ok(new ResponseCore<bool>(true))
              : BadRequest(new ResponseCore<bool>(false, "Delete failed!"));

        }
    }
}
