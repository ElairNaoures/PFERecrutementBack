using Microsoft.AspNetCore.Mvc;
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
        Task<bool> DeleteJob(int jobId);
        Task<List<TabJob>> GetJobsByLetter(string letter);

        Task<IEnumerable<TabJob>> GetJobsByProfile(string profileName);
        IEnumerable<TabJob> GetJobsByProfileId(int profileId);
        Task<TabUser> GetUserById(int userId);
        Task<string?> GetUserEmailById(int userId);

    }
}
