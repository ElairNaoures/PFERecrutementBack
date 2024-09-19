using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.CompteRepo;
using QualiPro_Recruitment_Web_Api.Repositories.EducationRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationRepository _educationRepository;

        public EducationController(IEducationRepository educationRepository)
        {
            _educationRepository = educationRepository;
        }


        [HttpPost]
        public async Task<IActionResult> AddEducation([FromBody] EducationDto educationInput)
        {
            TabEducation Education = await _educationRepository.AddEducation(educationInput);
            return Ok(Education);


        }


       


        [HttpGet]
        public async Task<IActionResult> GetAllEducations()
        {
            var ListEducations = await _educationRepository.GetAllEducations();
            return Ok(ListEducations);

        }
        [HttpGet("{educationId}")]
        public async Task<IActionResult> GetEducationId(int educationId)
        {
            var Education = await _educationRepository.GetEducationById(educationId);
            return Ok(Education);


        }


        [HttpPut("{educationId}")]
        public async Task<IActionResult> UpdateEducation([FromRoute] int educationId, [FromBody] TabEducation educationInput)
        {

            var Education = await _educationRepository.UpdateEducation(educationId, educationInput);
            return Ok(Education);

        }

        [HttpDelete("{educationId}")]
        public async Task<IActionResult> DeleteEducation(int educationId)
        {
            var deletedEducation = await _educationRepository.DeleteEducation(educationId);
            if (deletedEducation == null)
            {
                return NotFound("Education not found");
            }
            return Ok("Education deleted successfully");
        }


     



        [HttpGet("condidat/{condidatId}")]
        public async Task<IActionResult> GetEducationsByCondidatId(int condidatId)
        {
            var educations = await _educationRepository.GetEducationsByCondidatId(condidatId);
            if (educations == null || !educations.Any())
            {
                return NotFound("No Educations found for this candidate");
            }
            return Ok(educations);
        }

    }
}

 
