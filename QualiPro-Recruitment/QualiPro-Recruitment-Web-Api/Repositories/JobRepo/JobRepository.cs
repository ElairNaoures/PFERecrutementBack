using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
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
            TabJob job = new TabJob()
            {

                Title = jobInput.Title,
                Description = jobInput.Description,
                //UserId = jobInput.UserId,
                YearsOfExperience = jobInput.YearsOfExperience,
                Languages = jobInput.Languages,
                EducationLevel = jobInput.EducationLevel,
                ContractTypeId = jobInput.ContractTypeId,
                ExpirationDate = jobInput.ExpirationDate,
                CreatedAt = DateTime.Now,
                //ContractTypeId = jobInput.ContractTypeId,
                UserId = jobInput.UserId,
               


            };


            await _qualiProContext.AddAsync(job);
            await _qualiProContext.SaveChangesAsync();
            return job;
        }


        public async Task<List<TabJob>> GetAllJobs()
        {
            var ListJobs = await _qualiProContext.TabJobs.OrderByDescending(p=>p.Id).ToListAsync();
            return ListJobs;
        }

        public async Task<TabJob> GetJobById(int jobId)
        {
            var Job = await _qualiProContext.TabJobs.FirstOrDefaultAsync(p => p.Id == jobId);
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






            await _qualiProContext.SaveChangesAsync();
            return job;
        }

        public async Task<TabJob> DeleteJob(int jobId)
        {
            var job = await _qualiProContext.TabJobs.FindAsync(jobId);
            if (job == null)
            {
                return null;
            }
            _qualiProContext.TabJobs.Remove(job);
            await _qualiProContext.SaveChangesAsync();
            return job;
        }


    }
}
