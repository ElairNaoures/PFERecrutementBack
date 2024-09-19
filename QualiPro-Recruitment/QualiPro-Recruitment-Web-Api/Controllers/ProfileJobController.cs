using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.ProfileJobRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileJobController : ControllerBase
    {
        private readonly IProfileJobRepository _profileJobRepository;

        public ProfileJobController(IProfileJobRepository profileJobRepository)
        {
            _profileJobRepository = profileJobRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileJobDto>>> GetAllProfileJobs()
        {
            var profileJobs = await _profileJobRepository.GetAllProfileJobs();
            var profileJobDtos = profileJobs.Select(pj => new ProfileJobDto
            {
                Id = pj.Id,
                ProfileName = pj.ProfileName,
                QuizId = pj.QuizId
            });

            return Ok(profileJobDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileJobDto>> GetProfileJobById(int id)
        {
            var profileJob = await _profileJobRepository.GetProfileJobById(id);
            if (profileJob == null) return NotFound();

            var profileJobDto = new ProfileJobDto
            {
                Id = profileJob.Id,
                ProfileName = profileJob.ProfileName,
                QuizId = profileJob.QuizId
            };

            return Ok(profileJobDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileJobDto>> AddProfileJob(ProfileJobDto profileJobDto)
        {
            var profileJob = new TabProfileJob
            {
                ProfileName = profileJobDto.ProfileName,
                QuizId = profileJobDto.QuizId
            };

            var createdProfileJob = await _profileJobRepository.AddProfileJob(profileJob);

            profileJobDto.Id = createdProfileJob.Id;
            return CreatedAtAction(nameof(GetProfileJobById), new { id = profileJobDto.Id }, profileJobDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfileJob(int id, ProfileJobDto profileJobDto)
        {
            var profileJob = new TabProfileJob
            {
                Id = id,
                ProfileName = profileJobDto.ProfileName,
                QuizId = profileJobDto.QuizId
            };

            var updatedProfileJob = await _profileJobRepository.UpdateProfileJob(id, profileJob);
            if (updatedProfileJob == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfileJob(int id)
        {
            var result = await _profileJobRepository.DeleteProfileJob(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
