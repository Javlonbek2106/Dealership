using Application.DTOs.Role;
using Application.Interfaces.ModelInterface.Login;
using Application.ResponseModel;
using Domain.Entities.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceCatalogUI.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceCatalogUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class RoleController : ApiControllerBase<Role>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        public RoleController(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }
        [HttpPost("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "CreateRole")]
        public async Task<ActionResult<ResponseCore<RoleGetDTO>>> Create([FromBody] RoleCreateDTO role)
        {
            Role mappedRole = _mapper.Map<Role>(role);
            var validationResult = _validator.Validate(mappedRole);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<object>(false, validationResult.Errors));
            }
            mappedRole.Permissions = new List<Permission>();
            foreach (Guid item in role.PermissionsId)
            {
                Permission? permission = await _permissionRepository.GetByIdAsync(item);
                if (permission != null)
                    mappedRole.Permissions.Add(permission);
                else return BadRequest(new ResponseCore<string>(false, item + " Id not found"));
            }
            mappedRole = await _roleRepository.CreateAsync(mappedRole);
            var res = _mapper.Map<RoleGetDTO>(mappedRole);
            return Ok(new ResponseCore<object>(res));
        }

        [HttpGet("[action]")]
        //[Authorize(Roles = "GetAllRole")]
        public async Task<ActionResult<ResponseCore<IEnumerable<RoleGetDTO>>>> GetAllRole(string searchQuery = null)
        {
            IEnumerable<Role> roles;

            if (searchQuery != null)
            {
                roles = await _roleRepository.GetAsync(x =>
                    string.IsNullOrEmpty(searchQuery) || x.RoleName.Contains(searchQuery), nameof(Role.Permissions));
            }
            else
            {
                roles = await _roleRepository.GetAsync(x => true, nameof(Role.Permissions));
            }

            IEnumerable<RoleGetDTO> mappedRoles = _mapper.Map<IEnumerable<RoleGetDTO>>(roles);

            return Ok(new ResponseCore<IEnumerable<RoleGetDTO>>(mappedRoles));
        }


        [HttpGet("[action]")]
        //[Authorize(Roles = "GetByIdRole")]
        public async Task<ActionResult<ResponseCore<RoleGetDTO>>> GetById(Guid id)
        {
            IEnumerable<Role> roles = await _roleRepository.GetAsync(x => true, nameof(Role.Permissions));
            Role? role = roles.FirstOrDefault(x=>x.Id==id);
            if (role == null)
            {
                return NotFound(new ResponseCore<Role?>(false, id + " not found!"));
            }
            RoleGetDTO mappedRole = _mapper.Map<RoleGetDTO>(role);
            return Ok(new ResponseCore<RoleGetDTO?>(mappedRole));
        }

        [HttpPut("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "UpdateRole")]
        public async Task<ActionResult<ResponseCore<RoleGetDTO>>> Update([FromBody] RoleUpdateDTO role)
        {
            Role? mappedRole = _mapper.Map<Role>(role);
            var validationResult = _validator.Validate(mappedRole);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<Role>(false, validationResult.Errors));
            }
            mappedRole.Permissions = new List<Permission>();
            foreach (var item in role.PermissionsId)
            {
                Permission? permission = await _permissionRepository.GetByIdAsync(item);
                if (permission != null)
                    mappedRole.Permissions.Add(permission);
                else return BadRequest(new ResponseCore<Role>(false, item + " Id not found"));
            }
            mappedRole = await _roleRepository.UpdateAsync(mappedRole, nameof(Role.Permissions));
            if (mappedRole != null)
                return Ok(new ResponseCore<RoleGetDTO>(_mapper.Map<RoleGetDTO>(mappedRole)));
            return BadRequest(new ResponseCore<Role>(false, role + " not found"));

        }

        

        [HttpDelete("[action]")]
        //[Authorize(Roles = "DeleteRole")]
        public async Task<ActionResult<ResponseCore<bool>>> Delete(Guid id)
        {
            return await _roleRepository.DeleteAsync(id) ?
                Ok(new ResponseCore<bool>(true))
              : BadRequest(new ResponseCore<bool>(false, "Delete failed!"));

        }

    }
}
