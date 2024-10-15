using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.JobRepo;
using QualiPro_Recruitment_Web_Api.Repositories.ModuleRepo;
using QualiPro_Recruitment_Web_Api.Services;

namespace QualiPro_Recruitment_Web_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {

        private readonly IJobRepository _jobRepository;
        private readonly EmailService _emailService;

        public JobController(IJobRepository jobRepository, EmailService emailService)
        {
            _jobRepository = jobRepository;
            _emailService = emailService;
        }

        //[HttpPost]
        //public async Task<IActionResult> AddJob([FromBody] JobDto jobInput)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        TabJob job = await _jobRepository.AddJob(jobInput);
        //        return Ok(job);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error: " + ex.Message);
        //    }
        //}



        [HttpPost]
        public async Task<IActionResult> AddJob([FromBody] JobDto jobInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Add the job
                TabJob job = await _jobRepository.AddJob(jobInput);

                // Check if a user is associated with the job
                if (job.UserId.HasValue)
                {
                    // Get user details by UserId
                    var user = await _jobRepository.GetUserById(job.UserId.Value);
                    if (user != null)
                    {
                        var email = user.TabAccounts.FirstOrDefault()?.Email; // Get the associated account's email
                        if (!string.IsNullOrEmpty(email))
                        {
                            // Prepare the email body
                            string emailSubject = "Affectation d'un nouveau job";
                            string emailBody = $@"
                    <html>
                    <body>
                        <p>Bonjour {user.FirstName},</p>
                        <p>Un nouveau job vous a été affecté :</p>
                        <ul>
                            <li><strong>Nom du job</strong> : {job.Title}</li>
                            <li><strong>Description</strong> : {job.Description}</li>
                        </ul>
                        <p>Merci de vérifier les détails et de commencer à travailler sur ce job.</p>
                        <p>Cordialement,</p>
                        <p>L'équipe QualiPro</p>
                    </body>
                    </html>";

                            // Send the email
                            await _emailService.SendEmailAsync(email, emailSubject, emailBody);
                        }
                        else
                        {
                            return BadRequest("L'utilisateur n'a pas d'adresse e-mail associée.");
                        }
                    }
                    else
                    {
                        return BadRequest("Aucun utilisateur trouvé avec cet ID.");
                    }
                }
                else
                {
                    return BadRequest("Aucun utilisateur n'est associé à ce job.");
                }

                return Ok(job);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur : " + ex.Message);
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
