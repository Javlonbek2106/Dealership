using Application.DTOs.Role;
using Microsoft.AspNetCore.Mvc;
using ServiceCatalogUI.Filters;
using Application.ResponseModel;
using Application.DTOs.Permission;
using Domain.Entities.IdentityEntities;
using Application.Interfaces.ModelInterface.Login;

namespace ServiceCatalogUI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ApiControllerBase<Permission>
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
            _permissionRepository = permissionRepository;
            _permissionRepository = permissionRepository;
        }
        [HttpPost("[action]")]
        [ActionModelValidation]
        //[Authorize(Roles = "CreatePermission")]
        public async Task<ActionResult<ResponseCore<RoleGetDTO>>> CreatePermission([FromBody] PermissionCreateDTO permission)
        {
            Permission mappedPermission = _mapper.Map<Permission>(permission);
            var validationResult = _validator.Validate(mappedPermission);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseCore<object>(false, validationResult.Errors));
            }

            mappedPermission = await _permissionRepository.CreateAsync(mappedPermission);
            return Ok(new ResponseCore<object>(mappedPermission));
        }

        [HttpGet("[action]")]
        //[Authorize(Roles = "getallpermission")]
        public async Task<ActionResult<ResponseCore<IEnumerable<PermissionGetDTO>>>> GetAllPermission(string searchQuery = null)
        {
            await Console.Out.WriteLineAsync("permission");
            IEnumerable<Permission> permissions;

            if (searchQuery != null)
            {
                permissions = await _permissionRepository.GetAsync(x =>
                    string.IsNullOrEmpty(searchQuery) || x.PermissionName.Contains(searchQuery));
            }
            else
            {
                permissions = await _permissionRepository.GetAsync(x => true);
            }

            IEnumerable<PermissionGetDTO> mappedPermissions = _mapper.Map<IEnumerable<PermissionGetDTO>>(permissions);

            return Ok(new ResponseCore<IEnumerable<PermissionGetDTO>>(mappedPermissions));
        }


        [HttpDelete("[action]")]
        //[Authorize(Roles = "DeletePermission")]
        public async Task<ActionResult<ResponseCore<bool>>> DeletePermission(Guid id)
        {
            return await _permissionRepository.DeleteAsync(id) ?
                Ok(new ResponseCore<bool>(true))
              : BadRequest(new ResponseCore<bool>(false, "Delete failed!"));
        }
    }
}
