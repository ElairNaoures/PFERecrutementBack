using Microsoft.AspNetCore.Mvc;
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

            TabJob Job = await _jobRepository.AddJob(jobInput);
            return Ok(Job);
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

    }
}
