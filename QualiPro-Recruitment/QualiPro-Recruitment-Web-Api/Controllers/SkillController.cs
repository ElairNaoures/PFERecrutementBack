using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.Repositories.SkillRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillRepository _skillRepository;

        public SkillController(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkills()
        {
            var skills = await _skillRepository.GetAllSkills();
            return Ok(skills);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkill(int id)
        {
            var skill = await _skillRepository.GetSkillById(id);
            if (skill == null)
            {
                return NotFound();
            }
            return Ok(skill);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] TabSkill skill)
        {
            if (skill == null)
            {
                return BadRequest();
            }

            var createdSkill = await _skillRepository.CreateSkill(skill);
            return CreatedAtAction(nameof(GetSkill), new { id = createdSkill.Id }, createdSkill);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] TabSkill skill)
        {
            if (skill == null || skill.Id != id)
            {
                return BadRequest();
            }

            var updatedSkill = await _skillRepository.UpdateSkill(skill);
            if (updatedSkill == null)
            {
                return NotFound();
            }

            return Ok(updatedSkill);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var deleted = await _skillRepository.DeleteSkill(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
