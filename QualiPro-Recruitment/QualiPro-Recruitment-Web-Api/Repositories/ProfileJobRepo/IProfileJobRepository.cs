using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.ProfileJobRepo
{
    public interface IProfileJobRepository
    {
        Task<IEnumerable<TabProfileJob>> GetAllProfileJobs();
        Task<TabProfileJob?> GetProfileJobById(int id);
        Task<TabProfileJob> AddProfileJob(TabProfileJob profileJob);
        Task<TabProfileJob?> UpdateProfileJob(int id, TabProfileJob profileJob);
        Task<bool> DeleteProfileJob(int id);
    }
}
