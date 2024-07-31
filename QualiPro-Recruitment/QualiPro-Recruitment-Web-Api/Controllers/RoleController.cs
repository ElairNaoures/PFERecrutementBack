using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.Repositories.CompteRepo;
using QualiPro_Recruitment_Web_Api.Repositories.RoleRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] TabRole roleInput)
        {
            var Role = await _roleRepository.AddRole(roleInput);
            return Ok(Role);


        }


        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var ListRoles = await _roleRepository.GetAllRoles();
            return Ok(ListRoles);

        }
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleId(int roleId)
        {
            var Role = await _roleRepository.GetRoleById(roleId);
            return Ok(Role);


        }
        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole([FromRoute] int roleId, [FromBody] TabRole userInput)
        {
            var Role = await _roleRepository.UpdateRole(roleId, userInput);
            return Ok(Role);


        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var deletedRole = await _roleRepository.DeleteRole(roleId);
            if (deletedRole == null)
            {
                return NotFound("Role not found");
            }
            return Ok("Role deleted successfully");
        }

    }
}
