using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.JobApplicationRepo;
using QualiPro_Recruitment_Web_Api.Repositories.JobRepo;
using QualiPro_Recruitment_Web_Api.Repositories.NotificationRepo;
using QualiPro_Recruitment_Web_Api.Repositories.UserRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJobRepository _jobRepository;
        private readonly ILogger<JobApplicationController> _logger;


        public JobApplicationController(IJobApplicationRepository jobApplicationRepository,
            INotificationRepository notificationRepository,
            IUserRepository userRepository, IJobRepository jobRepository, ILogger<JobApplicationController> logger // Ajouter le paramètre ILogger
)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _jobRepository = jobRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllJobApplications()
        {
            var jobApplications = await _jobApplicationRepository.GetAllJobApplicationsAsync();
            var jobApplicationDtos = jobApplications.Select(ja => new JobApplicationDto
            {
                Id = ja.Id,
                CondidatId = ja.CondidatId,
                JobId = ja.JobId,
                MeetingDate = ja.MeetingDate,
                HeadToHeadInterviewNote = ja.HeadToHeadInterviewNote,
                Score = ja.Score // Assurez-vous que Score est correctement assigné
            });
            return Ok(jobApplicationDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationDto>> GetJobApplicationById(int id)
        {
            var jobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
            if (jobApplication == null)
                return NotFound();

            var jobApplicationDto = new JobApplicationDto
            {
                Id = jobApplication.Id,
                CondidatId = jobApplication.CondidatId,
                JobId = jobApplication.JobId,
                MeetingDate = jobApplication.MeetingDate,
                HeadToHeadInterviewNote = jobApplication.HeadToHeadInterviewNote,
                Score = jobApplication.Score // Assurez-vous que Score est correctement assigné
            };

            return Ok(jobApplicationDto);
        }

        [HttpPost]
        public async Task<ActionResult<JobApplicationDto>> AddJobApplication(JobApplicationDto jobApplicationDto)
        {
            if (!jobApplicationDto.CondidatId.HasValue || !jobApplicationDto.JobId.HasValue)
            {
                return BadRequest(new { message = "CondidatId and JobId are required and must be greater than zero." });
            }

            var jobApplication = new TabJobApplication
            {
                CondidatId = jobApplicationDto.CondidatId.Value,
                JobId = jobApplicationDto.JobId.Value,
                MeetingDate = jobApplicationDto.MeetingDate,
                HeadToHeadInterviewNote = jobApplicationDto.HeadToHeadInterviewNote,
                Score = jobApplicationDto.Score
            };

            var createdJobApplication = await _jobApplicationRepository.AddJobApplicationAsync(jobApplication);

            var createdJobApplicationDto = new JobApplicationDto
            {
                Id = createdJobApplication.Id,
                CondidatId = createdJobApplication.CondidatId,
                JobId = createdJobApplication.JobId,
                MeetingDate = createdJobApplication.MeetingDate,
                HeadToHeadInterviewNote = createdJobApplication.HeadToHeadInterviewNote,
                Score = createdJobApplication.Score
            };

            // Assurez-vous que JobId n'est pas null avant d'appeler GetJobById
            if (createdJobApplication.JobId.HasValue)
            {
                var job = await _jobRepository.GetJobById(createdJobApplication.JobId.Value);

                if (job == null)
                {
                    return NotFound(new { message = "Job not found." });
                }

                int userId = job.UserId ?? 0;

                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                var notification = new Notification
                {
                    UserId = user.Id,
                    CondidatId = createdJobApplication.CondidatId ?? 0,
                    Message = $"A new candidate has applied for the job: {job.Title}.",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                _logger.LogInformation("Creating notification for user {UserId} with message: {Message}", user.Id, notification.Message);

                await _notificationRepository.AddNotificationAsync(notification);
            }
            else
            {
                return BadRequest(new { message = "JobId cannot be null." });
            }

            return CreatedAtAction(nameof(GetJobApplicationById), new { id = createdJobApplicationDto.Id }, createdJobApplicationDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobApplication(int id, [FromBody] JobApplicationDto jobApplicationDto)
        {
            if (id != jobApplicationDto.Id)
                return BadRequest("JobApplication ID mismatch.");

            var existingJobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
            if (existingJobApplication == null)
                return NotFound();

            existingJobApplication.MeetingDate = jobApplicationDto.MeetingDate ?? existingJobApplication.MeetingDate;
            existingJobApplication.HeadToHeadInterviewNote = jobApplicationDto.HeadToHeadInterviewNote ?? existingJobApplication.HeadToHeadInterviewNote;
            existingJobApplication.Score = jobApplicationDto.Score; // Pas besoin de vérifier les valeurs nulles ici

            try
            {
                await _jobApplicationRepository.UpdateJobApplicationAsync(existingJobApplication);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _jobApplicationRepository.GetJobApplicationByIdAsync(id) == null)
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobApplication(int id)
        {
            var result = await _jobApplicationRepository.DeleteJobApplicationAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("count/{jobId}")]
        public async Task<ActionResult<int>> GetApplicationCountByJobId(int jobId)
        {
            var count = await _jobApplicationRepository.GetApplicationCountByJobIdAsync(jobId);
            return Ok(count);
        }

        [HttpGet("jobs-with-application-count")]
        public async Task<ActionResult<IEnumerable<object>>> GetJobsWithApplicationCount()
        {
            var jobApplications = await _jobApplicationRepository.GetJobsWithApplicationCount();

            var jobs = jobApplications
                .GroupBy(ja => ja.JobId)
                .Select(group => new
                {
                    JobId = group.Key,
                    Title = group.FirstOrDefault()?.Job?.Title,
                    ApplicationCount = group.Count()
                })
                .ToList();

            return Ok(jobs);
        }

        [HttpGet("candidates-with-score-above-threshold")]
        public async Task<ActionResult<IEnumerable<object>>> GetCandidatesWithScoreAboveThreshold()
        {
            const int scoreThreshold = 12; // Score threshold

            var jobApplications = await _jobApplicationRepository.GetCandidatesWithScoreAboveThreshold();

            var candidatesAboveThreshold = jobApplications
                .Where(ja => ja.Score >= scoreThreshold
                             && (!ja.Deleted.HasValue || !ja.Deleted.Value)
                             && (ja.Condidat == null || !ja.Condidat.Deleted.HasValue || !ja.Condidat.Deleted.Value))
                .GroupBy(ja => ja.Job != null ? ja.Job.Title : "Unknown") // Group by job title
                .Select(g => new
                {
                    JobTitle = g.Key,
                    Candidates = g.Select(ja => new
                    {
                        CandidateName = ja.Condidat != null ? ja.Condidat.FirstName : "Unknown",
                        Score = ja.Score
                    }).ToList()
                })
                .ToList();

            return Ok(candidatesAboveThreshold);
        }


        [HttpGet("job/{jobId}")]
        public async Task<ActionResult<object>> GetJobApplicationDetailsByJobId(int jobId)
        {
            var job = await _jobRepository.GetJobById(jobId);
            if (job == null)
                return NotFound(new { message = "Job not found." });

            var jobApplications = await _jobApplicationRepository.GetJobApplicationsByJobIdAsync(jobId);
            int applicationCount = jobApplications.Count(); // Compte des candidatures

            var bestCandidate = jobApplications
                .Where(ja => ja.Condidat != null && (!ja.Deleted.HasValue || !ja.Deleted.Value))
                .OrderByDescending(ja => ja.Score)
                .Select(ja => new
                {
                    CandidateName = ja.Condidat.FirstName + " " + ja.Condidat.LastName, // Combinaison du prénom et du nom
                    Score = ja.Score
                })
                .FirstOrDefault();

            return Ok(new
            {
                JobTitle = job.Title,
                ApplicationCount = applicationCount,
                BestCandidate = bestCandidate
            });
        }

    }




}
