using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.JobApplicationRepo
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<TabJobApplication>> GetAllJobApplicationsAsync();
        Task<TabJobApplication> GetJobApplicationByIdAsync(int id);
        Task<TabJobApplication> AddJobApplicationAsync(TabJobApplication jobApplication);
        Task<TabJobApplication> UpdateJobApplicationAsync(TabJobApplication jobApplication);
        Task<bool> DeleteJobApplicationAsync(int id);
        Task<int> GetApplicationCountByJobIdAsync(int jobId);
        Task<IEnumerable<TabJobApplication>> GetJobsWithApplicationCount();
        Task<IEnumerable<TabJobApplication>> GetCandidatesWithScoreAboveThreshold();

    }
}
