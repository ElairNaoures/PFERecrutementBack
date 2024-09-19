using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.ProfileJobRepo
{
    public class ProfileJobRepository : IProfileJobRepository
    {
        private readonly QualiProContext _qualiProContext;

        public ProfileJobRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
        }

        public async Task<IEnumerable<TabProfileJob>> GetAllProfileJobs()
        {
            return await _qualiProContext.TabProfileJobs.OrderByDescending(p => p.Id)
                .Include(pj => pj.Quiz)
                .Include(pj => pj.TabJobs)
                .ToListAsync();
        }

        public async Task<TabProfileJob?> GetProfileJobById(int id)
        {
            return await _qualiProContext.TabProfileJobs
                .Include(pj => pj.Quiz)
                .Include(pj => pj.TabJobs)
                .FirstOrDefaultAsync(pj => pj.Id == id);
        }

        public async Task<TabProfileJob> AddProfileJob(TabProfileJob profileJob)
        {
            _qualiProContext.TabProfileJobs.Add(profileJob);
            await _qualiProContext.SaveChangesAsync();
            return profileJob;
        }

        public async Task<TabProfileJob?> UpdateProfileJob(int id, TabProfileJob profileJob)
        {
            var existingProfileJob = await _qualiProContext.TabProfileJobs.FindAsync(id);
            if (existingProfileJob == null) return null;

            existingProfileJob.ProfileName = profileJob.ProfileName;
            existingProfileJob.QuizId = profileJob.QuizId;

            await _qualiProContext.SaveChangesAsync();
            return existingProfileJob;
        }

        public async Task<bool> DeleteProfileJob(int id)
        {
            var profileJob = await _qualiProContext.TabProfileJobs.FindAsync(id);
            if (profileJob == null) return false;

            _qualiProContext.TabProfileJobs.Remove(profileJob);
            await _qualiProContext.SaveChangesAsync();
            return true;
        }
    }
}
