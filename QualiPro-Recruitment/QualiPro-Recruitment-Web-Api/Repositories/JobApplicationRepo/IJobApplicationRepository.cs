using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
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
        // Task<IEnumerable<TabJobApplication>> GetCandidatesWithScoreAboveThreshold();
        Task<IEnumerable<TabJobApplication>> GetCandidatesWithScoreAboveThreshold();
        Task<IEnumerable<TabJobApplication>> GetJobApplicationsByJobIdAsync(int jobId);
        Task<IEnumerable<TabCondidat>> GetCandidatesByJobApplicationIdAsync(int jobApplicationId);

        Task<CondidatDto?> GetCondidatInfo(int condidatId, int jobId);
        Task<TabCondidat> GetCondidatById(int condidatId);
        Task<string?> GetCondidatEmailById(int? condidatId);
        Task<string?> GetCondidatNameById(int? condidatId);

    }
}
