using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.JobRepo
{
    public interface IJobRepository
    {
        Task<TabJob> AddJob(JobDto jobInput);
        Task<List<TabJob>> GetAllJobs();

        Task<TabJob> GetJobById(int jobId);
        Task<TabJob> UpdateJob(int jobId, TabJob jobInput);
        Task<TabJob> DeleteJob(int jobId);
    }
}
