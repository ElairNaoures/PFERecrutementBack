using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using System.Diagnostics.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualiPro_Recruitment_Web_Api.Repositories.JobRepo
{
    public class JobRepository : IJobRepository
    {


        private readonly QualiProContext _qualiProContext;

        public JobRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }


        public async Task<TabJob> AddJob(JobDto jobInput)
        {
            TabJob job = new TabJob
            {
                Title = jobInput.Title,
                Description = jobInput.Description,
                YearsOfExperience = jobInput.YearsOfExperience,
                Languages = jobInput.Languages,
                EducationLevel = jobInput.EducationLevel,
                ContractTypeId = jobInput.ContractTypeId,
                ExpirationDate = jobInput.ExpirationDate,
                CreatedAt = DateTime.Now,
                UserId = jobInput.UserId,
                JobProfileId = jobInput.JobProfileId
            };

            await _qualiProContext.AddAsync(job);
            await _qualiProContext.SaveChangesAsync();
            return job;
        }


        public async Task<List<TabJob>> GetAllJobs()
        {
            var ListJobs = await _qualiProContext.TabJobs
                .Where(p => p.Deleted == false || p.Deleted == null)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return ListJobs;
        }

        public async Task<TabJob> GetJobById(int jobId)
        {
            var Job = await _qualiProContext.TabJobs
                .FirstOrDefaultAsync(p => p.Id == jobId && (p.Deleted == false || p.Deleted == null));

            return Job;
        }


       

        public async Task<TabJob> UpdateJob(int jobId, TabJob jobInput)
        {
            var job = await _qualiProContext.TabJobs.FirstOrDefaultAsync(p => p.Id == jobId);



            job.Title = jobInput.Title;
            job.Description = jobInput.Description;
            job.UserId = jobInput.UserId;
            job.YearsOfExperience = jobInput.YearsOfExperience;
            job.Languages = jobInput.Languages;
            job.EducationLevel = jobInput.EducationLevel;
            job.ContractTypeId = jobInput.ContractTypeId;
            job.ExpirationDate = jobInput.ExpirationDate;
            job.CreatedAt = DateTime.Now;
            job.JobProfileId = jobInput.JobProfileId;






            await _qualiProContext.SaveChangesAsync();
            return job;
        }
        public async Task<bool> DeleteJob(int jobId)
        {
            var job = await _qualiProContext.TabJobs.FindAsync(jobId);
            if (job == null)
            {
                return false;
            }

            job.Deleted = true;

            await _qualiProContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<TabJob>> GetJobsByLetter(string letter)
        {
            return await _qualiProContext.TabJobs
                .Where(j => j.Title.StartsWith(letter) && (j.Deleted == false || j.Deleted == null))
                .ToListAsync();
        }


        public async Task<IEnumerable<TabJob>> GetJobsByProfile(string profileName)
        {
            return await _qualiProContext.TabJobs
                .Include(job => job.JobProfile)
                .Where(job => job.JobProfile.ProfileName == profileName && (job.Deleted == false || job.Deleted == null))
                .ToListAsync();
        }




        public IEnumerable<TabJob> GetJobsByProfileId(int profileId)
        {
            return _qualiProContext.TabJobs
                .Where(job => job.JobProfileId == profileId && (job.Deleted == false || job.Deleted == null))
                .ToList();
        }

        public async Task<TabUser> GetUserById(int userId)
        {
            return await _qualiProContext.TabUsers
                .Include(u => u.TabAccounts)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<string?> GetUserEmailById(int userId)
        {
            var user = await GetUserById(userId);
            var account = user.TabAccounts.FirstOrDefault(); 
            return account?.Email; 
        }



    }
}
