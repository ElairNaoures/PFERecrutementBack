using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Models;
using QualiPro_Recruitment_Web_Api.Repositories.JobApplicationRepo;
using QualiPro_Recruitment_Web_Api.Repositories.JobRepo;
using QualiPro_Recruitment_Web_Api.Repositories.NotificationRepo;
using QualiPro_Recruitment_Web_Api.Repositories.UserRepo;
using QualiPro_Recruitment_Web_Api.Services;
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
        private readonly EmailService _emailService;



        public JobApplicationController(IJobApplicationRepository jobApplicationRepository, EmailService emailService,
            INotificationRepository notificationRepository,
            IUserRepository userRepository, IJobRepository jobRepository, ILogger<JobApplicationController> logger // Ajouter le paramètre ILogger
)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _jobRepository = jobRepository;
            _logger = logger;
            _emailService = emailService;

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


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateJobApplication(int id, [FromBody] JobApplicationDto jobApplicationDto)
        //{
        //    if (id != jobApplicationDto.Id)
        //        return BadRequest("JobApplication ID mismatch.");

        //    var existingJobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
        //    if (existingJobApplication == null)
        //        return NotFound();

        //    existingJobApplication.MeetingDate = jobApplicationDto.MeetingDate ?? existingJobApplication.MeetingDate;
        //    existingJobApplication.HeadToHeadInterviewNote = jobApplicationDto.HeadToHeadInterviewNote ?? existingJobApplication.HeadToHeadInterviewNote;
        //    existingJobApplication.Score = jobApplicationDto.Score; // Pas besoin de vérifier les valeurs nulles ici

        //    try
        //    {
        //        await _jobApplicationRepository.UpdateJobApplicationAsync(existingJobApplication);
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (await _jobApplicationRepository.GetJobApplicationByIdAsync(id) == null)
        //            return NotFound();

        //        throw;
        //    }

        //    return NoContent();
        //}
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> UpdateJobApplication(int id, [FromBody] JobApplicationDto jobApplicationDto)
        //    {
        //        if (id != jobApplicationDto.Id)
        //            return BadRequest("JobApplication ID mismatch.");

        //        var existingJobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
        //        if (existingJobApplication == null)
        //            return NotFound();

        //        // Store the old meeting date for comparison
        //        var oldMeetingDate = existingJobApplication.MeetingDate;

        //        existingJobApplication.MeetingDate = jobApplicationDto.MeetingDate ?? existingJobApplication.MeetingDate;
        //        existingJobApplication.HeadToHeadInterviewNote = jobApplicationDto.HeadToHeadInterviewNote ?? existingJobApplication.HeadToHeadInterviewNote;
        //        existingJobApplication.Score = jobApplicationDto.Score;

        //        try
        //        {
        //            await _jobApplicationRepository.UpdateJobApplicationAsync(existingJobApplication);


        //            // Check if MeetingDate was actually updated
        //            if (oldMeetingDate != existingJobApplication.MeetingDate && existingJobApplication.CondidatId.HasValue)
        //            {
        //                // Get the email and name of the candidate
        //                var candidateEmail = await _jobApplicationRepository.GetCondidatEmailById(existingJobApplication.CondidatId.Value);
        //                var candidateName = await _jobApplicationRepository.GetCondidatNameById(existingJobApplication.CondidatId.Value);

        //                // Format the MeetingDate to display only the date (without time)
        //                string formattedMeetingDate = existingJobApplication.MeetingDate?.ToString("dd/MM/yyyy");

        //                // HTML formatted email message with enhanced styling
        //                string emailBody = $@"
        //<html>
        //<head>
        //    <style>
        //        body {{
        //            font-family: Arial, sans-serif;
        //            margin: 0;
        //            padding: 20px;
        //            background-color: #f4f4f4;
        //        }}
        //        .container {{
        //            max-width: 600px;
        //            margin: auto;
        //            background-color: #ffffff;
        //            padding: 20px;
        //            border-radius: 8px;
        //            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        //        }}
        //        h2 {{
        //            color: #2c3e50;
        //            text-align: center;
        //        }}
        //        p {{
        //            line-height: 1.6;
        //            color: #333;
        //        }}
        //        .highlight {{
        //            color: #e74c3c; /* Red color for emphasis */
        //            font-weight: bold;
        //        }}
        //        .footer {{
        //            margin-top: 20px;
        //            text-align: center;
        //            font-size: 0.9em;
        //            color: #777;
        //        }}
        //    </style>
        //</head>
        //<body>
        //    <div class='container'>
        //        <h2>Félicitations!</h2>
        //        <p>Bonjour cher {candidateName},</p>
        //        <p>Nous avons le plaisir de vous informer que vous avez été sélectionné pour passer à l'étape suivante de notre processus de recrutement.</p>
        //        <p>Votre entretien est prévu pour le <span class='highlight'>{formattedMeetingDate}</span>. </p>
        //        <p>Nous sommes impatients de vous rencontrer!</p>
        //        <p>Merci de votre intérêt et de votre participation.</p>
        //        <div class='footer'>
        //            <p>Cordialement,<br/>
        //            L'équipe de recrutement</p>
        //        </div>
        //    </div>
        //</body>
        //</html>";

        //                // Send the email
        //                await _emailService.SendEmailAsync(candidateEmail, "Date d'entretient", emailBody);
        //            }

        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (await _jobApplicationRepository.GetJobApplicationByIdAsync(id) == null)
        //                return NotFound();

        //            throw;
        //        }

        //        return NoContent();
        //    }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobApplication(int id, [FromBody] JobApplicationDto jobApplicationDto)
        {
            if (id != jobApplicationDto.Id)
                return BadRequest("JobApplication ID mismatch.");

            var existingJobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
            if (existingJobApplication == null)
                return NotFound();

            var oldMeetingDate = existingJobApplication.MeetingDate;

            existingJobApplication.MeetingDate = jobApplicationDto.MeetingDate ?? existingJobApplication.MeetingDate;
            existingJobApplication.HeadToHeadInterviewNote = jobApplicationDto.HeadToHeadInterviewNote ?? existingJobApplication.HeadToHeadInterviewNote;
            existingJobApplication.Score = jobApplicationDto.Score;

            try
            {
                await _jobApplicationRepository.UpdateJobApplicationAsync(existingJobApplication);

                if (oldMeetingDate != existingJobApplication.MeetingDate && existingJobApplication.CondidatId.HasValue)
                {
                    var candidateEmail = await _jobApplicationRepository.GetCondidatEmailById(existingJobApplication.CondidatId.Value);
                    var candidateName = await _jobApplicationRepository.GetCondidatNameById(existingJobApplication.CondidatId.Value);

                    string formattedMeetingDate = existingJobApplication.MeetingDate?.ToString("dd/MM/yyyy");

                    // Generate the quiz link using the job application ID
                    var quizLink = $"http://localhost:4200/jobs/CandidateQuiz/{existingJobApplication.Id}";

                    // HTML formatted email message with quiz link included
                    string emailBody = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                margin: 0;
                padding: 20px;
                background-color: #f4f4f4;
            }}
            .container {{
                max-width: 600px;
                margin: auto;
                background-color: #ffffff;
                padding: 20px;
                border-radius: 8px;
                box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            }}
            h2 {{
                color: #2c3e50;
                text-align: center;
            }}
            p {{
                line-height: 1.6;
                color: #333;
            }}
            .highlight {{
                color: #e74c3c; /* Red color for emphasis */
                font-weight: bold;
            }}
            .footer {{
                margin-top: 20px;
                text-align: center;
                font-size: 0.9em;
                color: #777;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Félicitations!</h2>
            <p>Bonjour cher {candidateName},</p>
            <p>Nous avons le plaisir de vous informer que vous avez été sélectionné pour passer à l'étape suivante de notre processus de recrutement.</p>
            <p>Votre entretien est prévu pour le <span class='highlight'>{formattedMeetingDate}</span>. </p>
            <p>Merci de cliquer sur le lien ci-dessous pour passer le quiz nécessaire :</p>
            <p><a href='{quizLink}'>Passer le quiz</a></p>
            <p>Nous sommes impatients de vous rencontrer!</p>
            <div class='footer'>
                <p>Cordialement,<br/>
                L'équipe de recrutement</p>
            </div>
        </div>
    </body>
    </html>";

                    // Send the email
                    await _emailService.SendEmailAsync(candidateEmail, "Date d'entretien et lien de quiz", emailBody);
                }

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
        //[HttpGet("job/{jobId}/applications")]
        //public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllJobApplicationsByJobId(int jobId)
        //{
        //    var jobApplications = await _jobApplicationRepository.GetJobApplicationsByJobIdAsync(jobId);
        //    if (jobApplications == null || !jobApplications.Any())
        //    {
        //        return NotFound(new { message = "No job applications found for this job." });
        //    }

        //    var jobApplicationDtos = jobApplications.Select(ja => new JobApplicationDto
        //    {
        //        Id = ja.Id,
        //        CondidatId = ja.CondidatId,
        //        JobId = ja.JobId,
        //        MeetingDate = ja.MeetingDate,
        //        HeadToHeadInterviewNote = ja.HeadToHeadInterviewNote,
        //        Score = ja.Score // Assurez-vous que Score est correctement assigné
        //    });

        //    return Ok(jobApplicationDtos);
        //}

        [HttpGet("jobinfo/{jobId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllJobApplicationsByJobId(int jobId)
        {
            var jobApplications = await _jobApplicationRepository.GetJobApplicationsByJobIdAsync(jobId);

            var jobApplicationDtos = jobApplications.Select(ja => new
            {
                Id = ja.Id,
                CondidatId = ja.CondidatId,
                CandidateName = ja.Condidat != null ? $"{ja.Condidat.FirstName} {ja.Condidat.LastName}" : "Unknown",
                HeadToHeadInterviewNote = ja.HeadToHeadInterviewNote,
                Score = ja.Score
            }).ToList();

            return Ok(jobApplicationDtos);
        }


        //[HttpPost("accept/{id}")]
        //public async Task<IActionResult> AcceptCandidate(int id)
        //{
        //    try
        //    {
        //        // Get the job application by ID
        //        var jobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
        //        if (jobApplication == null)
        //            return NotFound("Job application not found.");

        //        // Get candidate email and name
        //        var candidateEmail = await _jobApplicationRepository.GetCondidatEmailById(jobApplication.CondidatId);
        //        var candidateName = await _jobApplicationRepository.GetCondidatNameById(jobApplication.CondidatId);

        //        if (string.IsNullOrEmpty(candidateEmail) || string.IsNullOrEmpty(candidateName))
        //            return BadRequest("Candidate information is incomplete.");

        //        // Generate the acceptance email body in French
        //        string emailBody = $@"
        //<html>
        //<body style='font-family: Arial, sans-serif; background-color: #f4f4f9; padding: 20px;'>
        //    <div style='max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; padding: 20px;'>
        //        <h2 style='color: #4CAF50;'>Félicitations {candidateName}!</h2>
        //        <p style='font-size: 16px;'>Nous sommes ravis de vous informer que vous avez été sélectionné(e) pour le poste auquel vous avez postulé.</p>
        //        <p style='font-size: 16px;'>Veuillez contacter notre équipe des ressources humaines pour plus de détails.</p>
        //        <p style='font-size: 16px;'>Nous avons hâte de collaborer avec vous!</p>
        //        <br/>
        //        <p style='font-size: 14px; color: #888;'>Cordialement,<br/>L'équipe de recrutement</p>
        //    </div>
        //</body>
        //</html>";

        //        // Send the acceptance email
        //        await _emailService.SendEmailAsync(candidateEmail, "Candidature Acceptée", emailBody);

        //        return Ok("Candidate accepted and email sent.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (consider using a logging framework)
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
        [HttpPost("accept/{id}")]
        public async Task<IActionResult> AcceptCandidate(int id)
        {
            try
            {
                var jobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
                if (jobApplication == null)
                    return NotFound("Job application not found.");

                var candidateEmail = await _jobApplicationRepository.GetCondidatEmailById(jobApplication.CondidatId);
                var candidateName = await _jobApplicationRepository.GetCondidatNameById(jobApplication.CondidatId);

                if (string.IsNullOrEmpty(candidateEmail) || string.IsNullOrEmpty(candidateName))
                    return BadRequest("Candidate information is incomplete.");

                string emailBody = $@"
            <html>
            <body style='font-family: Arial, sans-serif; background-color: #f4f4f9; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; padding: 20px;'>
                    <h2 style='color: #4CAF50;'>Félicitations {candidateName}!</h2>
                    <p style='font-size: 16px;'>Nous sommes ravis de vous informer que vous avez été sélectionné(e) pour le poste auquel vous avez postulé.</p>
                    <p style='font-size: 16px;'>Veuillez contacter notre équipe des ressources humaines pour plus de détails.</p>
                    <p style='font-size: 16px;'>Nous avons hâte de collaborer avec vous!</p>
                    <br/>
                    <p style='font-size: 14px; color: #888;'>Cordialement,<br/>L'équipe de recrutement</p>
                </div>
            </body>
            </html>";

                await _emailService.SendEmailAsync(candidateEmail, "Candidature Acceptée", emailBody);

                return Ok(new { message = "Candidate accepted and email sent." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost("reject/{id}")]
        //public async Task<IActionResult> RejectCandidate(int id)
        //{
        //    try
        //    {
        //        // Get the job application by ID
        //        var jobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
        //        if (jobApplication == null)
        //            return NotFound("Job application not found.");

        //        // Get candidate email and name
        //        var candidateEmail = await _jobApplicationRepository.GetCondidatEmailById(jobApplication.CondidatId);
        //        var candidateName = await _jobApplicationRepository.GetCondidatNameById(jobApplication.CondidatId);

        //        if (string.IsNullOrEmpty(candidateEmail) || string.IsNullOrEmpty(candidateName))
        //            return BadRequest("Candidate information is incomplete.");

        //        // Generate the rejection email body in French
        //        string emailBody = $@"
        //<html>
        //<body style='font-family: Arial, sans-serif; background-color: #f4f4f9; padding: 20px;'>
        //    <div style='max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; padding: 20px;'>
        //        <h2 style='color: #e74c3c;'>Cher(ère) {candidateName},</h2>
        //        <p style='font-size: 16px;'>Nous regrettons de vous informer que votre candidature n'a pas été retenue à cette occasion.</p>
        //        <p style='font-size: 16px;'>Nous vous remercions pour votre intérêt pour notre entreprise et vous encourageons à postuler à d'autres offres à l'avenir.</p>
        //        <p style='font-size: 16px;'>Nous vous souhaitons bonne chance dans vos futures démarches.</p>
        //        <br/>
        //        <p style='font-size: 14px; color: #888;'>Cordialement,<br/>L'équipe de recrutement</p>
        //    </div>
        //</body>
        //</html>";

        //        // Send the rejection email
        //        await _emailService.SendEmailAsync(candidateEmail, "Candidature Refusée", emailBody);

        //        return Ok("Candidate rejected and email sent.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (consider using a logging framework)
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectCandidate(int id)
        {
            try
            {
                var jobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(id);
                if (jobApplication == null)
                    return NotFound("Job application not found.");

                var candidateEmail = await _jobApplicationRepository.GetCondidatEmailById(jobApplication.CondidatId);
                var candidateName = await _jobApplicationRepository.GetCondidatNameById(jobApplication.CondidatId);

                if (string.IsNullOrEmpty(candidateEmail) || string.IsNullOrEmpty(candidateName))
                    return BadRequest("Candidate information is incomplete.");

                string emailBody = $@"
            <html>
            <body style='font-family: Arial, sans-serif; background-color: #f4f4f9; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; padding: 20px;'>
                    <h2 style='color: #e74c3c;'>Cher(ère) {candidateName},</h2>
                    <p style='font-size: 16px;'>Nous regrettons de vous informer que votre candidature n'a pas été retenue à cette occasion.</p>
                    <p style='font-size: 16px;'>Nous vous remercions pour votre intérêt pour notre entreprise et vous encourageons à postuler à d'autres offres à l'avenir.</p>
                    <p style='font-size: 16px;'>Nous vous souhaitons bonne chance dans vos futures démarches.</p>
                    <br/>
                    <p style='font-size: 14px; color: #888;'>Cordialement,<br/>L'équipe de recrutement</p>
                </div>
            </body>
            </html>";

                await _emailService.SendEmailAsync(candidateEmail, "Candidature Refusée", emailBody);

                return Ok(new { message = "Candidate rejected and email sent." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    

    [HttpGet("job-application/{jobApplicationId}/candidates")]
        public async Task<ActionResult<IEnumerable<object>>> GetCandidatesByJobApplicationId(int jobApplicationId)
        {
            var jobApplication = await _jobApplicationRepository.GetJobApplicationByIdAsync(jobApplicationId);
            if (jobApplication == null)
            {
                return NotFound(new { message = "Job application not found." });
            }

            var candidates = await _jobApplicationRepository.GetCandidatesByJobApplicationIdAsync(jobApplicationId);

            var candidateDetails = candidates.Select(c => new
            {
                c.Id,
                c.FirstName,
                c.LastName,
                c.Country,
                c.PhoneNumber,
                c.Birthdate,
                //c.Email,
                 c.Summary
            }).ToList();

            return Ok(candidateDetails);
        }
        [HttpGet("condidat/{condidatId}/job/{jobId}")]
        public async Task<IActionResult> GetCondidatInfo(int condidatId, int jobId)
        {
            var condidatInfo = await _jobApplicationRepository.GetCondidatInfo(condidatId, jobId);
            if (condidatInfo == null)
                return NotFound();

            return Ok(condidatInfo);
        }

    }




}
