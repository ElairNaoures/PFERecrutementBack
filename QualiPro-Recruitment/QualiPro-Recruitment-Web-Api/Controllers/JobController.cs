using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.JobRepo;
using QualiPro_Recruitment_Web_Api.Repositories.ModuleRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {

        private readonly IJobRepository _jobRepository;

        public JobController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddJob([FromBody] JobDto jobInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                TabJob job = await _jobRepository.AddJob(jobInput);
                return Ok(job);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }





        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var ListJobs = await _jobRepository.GetAllJobs();
            return Ok(ListJobs);

        }


        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJobId(int jobId)
        {
            var Job = await _jobRepository.GetJobById(jobId);
            return Ok(Job);


        }

        [HttpPut("{jobId}")]
        public async Task<IActionResult> UpdateJob([FromRoute] int jobId, [FromBody] TabJob jobInput)
        {
            var Job = await _jobRepository.UpdateJob(jobId, jobInput);
            return Ok(Job);


        }


        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteJob(int jobId)
        {
            var deletedJob = await _jobRepository.DeleteJob(jobId);
            if (deletedJob == null)
            {
                return NotFound("Job not found");
            }
            return Ok("Job deleted successfully");
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetJobsByLetter([FromQuery] string letter)
        {
            if (string.IsNullOrEmpty(letter))
            {
                return BadRequest("Letter is required");
            }

            var jobs = await _jobRepository.GetJobsByLetter(letter);
            return Ok(new { jobs, total = jobs.Count });
        }

        [HttpGet("by-profile")]
        public async Task<IActionResult> GetJobsByProfile([FromQuery] string profileName)
        {
            if (string.IsNullOrEmpty(profileName))
                return BadRequest("Profile name is required");

            var jobs = await _jobRepository.GetJobsByProfile(profileName);
            if (!jobs.Any())
                return NotFound("No jobs found for the specified profile name");

            return Ok(new { jobs, total = jobs.Count() });
        }

        [HttpGet("profile/{profileId}")]
        public IActionResult GetJobsByProfileId(int profileId)
        {
            var jobs = _jobRepository.GetJobsByProfileId(profileId);
            if (!jobs.Any())
                return NotFound("No jobs found for the specified profile ID");

            return Ok(jobs);
        }




    }
}
