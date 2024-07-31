using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.Repositories.ModuleRepo;
using QualiPro_Recruitment_Web_Api.Repositories.UserRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {

        private readonly IModuleRepository _moduleRepository;

        public ModuleController(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddModule([FromBody] TabModule moduleInput)
        {
            var Module = await _moduleRepository.AddModule(moduleInput);
            return Ok(Module);


        }
        [HttpGet]
        public async Task<IActionResult> GetAllModules()
        {
            var ListModules = await _moduleRepository.GetAllModules();
            return Ok(ListModules);

        }
        [HttpGet("{moduleId}")]
        public async Task<IActionResult> GetModuleId(int moduleId)
        {
            var Module = await _moduleRepository.GetModuleById(moduleId);
            return Ok(Module);


        }

        [HttpPut("{moduleId}")]
        public async Task<IActionResult> UpdateModule([FromRoute] int moduleId, [FromBody] TabModule moduleInput)
        {
            var Module = await _moduleRepository.UpdateModule(moduleId, moduleInput);
            return Ok(Module);


        }

        [HttpDelete("{moduleId}")]
        public async Task<IActionResult> DeleteModule(int moduleId)
        {
            var deletedModule = await _moduleRepository.DeleteModule(moduleId);
            if (deletedModule == null)
            {
                return NotFound("Module not found");
            }
            return Ok("Module deleted successfully");
        }


    }
}
