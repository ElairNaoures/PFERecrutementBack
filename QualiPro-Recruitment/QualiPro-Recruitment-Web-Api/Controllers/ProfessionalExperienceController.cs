using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.Repositories.CompteRepo;
using QualiPro_Recruitment_Web_Api.Repositories.ProfessionalExperienceRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalExperienceController : ControllerBase
    {
        private readonly IProfessionalExperienceRepository _professionalExperienceRepository;

        public ProfessionalExperienceController(IProfessionalExperienceRepository professionalExperienceRepository)
        {
            _professionalExperienceRepository = professionalExperienceRepository;
        }


        [HttpPost]
        public async Task<IActionResult> AddProfessionalExperience([FromBody] ProfessionalExperience professionalExperienceInput)
        {
            var ProfessionalExperience = await _professionalExperienceRepository.AddProfessionalExperience(professionalExperienceInput);
            return Ok(ProfessionalExperience);


        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfessionalExperiences()
        {
            var ListProfessionalExperiences = await _professionalExperienceRepository.GetAllProfessionalExperiences();
            return Ok(ListProfessionalExperiences);

        }
        [HttpGet("{professionalExperienceId}")]
        public async Task<IActionResult> GetProfessionalExperienceId(int professionalExperienceId)
        {
            var ProfessionalExperience = await _professionalExperienceRepository.GetProfessionalExperienceById(professionalExperienceId);
            return Ok(ProfessionalExperience);


        }


        [HttpPut("{professionalExperienceId}")]
        public async Task<IActionResult> UpdateProfessionalExperience([FromRoute] int professionalExperienceId, [FromBody] ProfessionalExperience professionalExperienceInput)
        {

            var ProfessionalExperience = await _professionalExperienceRepository.UpdateProfessionalExperience(professionalExperienceId, professionalExperienceInput);
            return Ok(ProfessionalExperience);

        }

        [HttpDelete("{professionalExperienceId}")]
        public async Task<IActionResult> DeleteProfessionalExperience(int professionalExperienceId)
        {
            var deletedProfessionalExperience = await _professionalExperienceRepository.DeleteProfessionalExperience(professionalExperienceId);
            if (deletedProfessionalExperience == null)
            {
                return NotFound("ProfessionalExperience not found");
            }
            return Ok("ProfessionalExperience deleted successfully");
        }


        [HttpGet("condidat/{condidatId}")]
        public async Task<IActionResult> GetProfessionalExperiencesByCondidatId(int condidatId)
        {
            var experiences = await _professionalExperienceRepository.GetProfessionalExperiencesByCondidatId(condidatId);
            if (experiences == null || !experiences.Any())
            {
                return NotFound("No Professional Experiences found for this candidate");
            }
            return Ok(experiences);
        }
    }
}
