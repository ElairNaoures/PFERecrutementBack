using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.JobApplicationRepo
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly QualiProContext _qualiProContext;

        public JobApplicationRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
        }

        public async Task<IEnumerable<TabJobApplication>> GetAllJobApplicationsAsync()
        {
            return await _qualiProContext.TabJobApplications.Where(p=>p.Deleted == false || p.Deleted == null).OrderByDescending(p => p.Id).ToListAsync();
        }

        public async Task<TabJobApplication> GetJobApplicationByIdAsync(int id)
        {
            return await _qualiProContext.TabJobApplications
                .Include(ja => ja.Job)

                .FirstOrDefaultAsync(ja => ja.Id == id
                                        && (ja.Deleted == false || ja.Deleted == null) // Filtrer les candidatures non supprimées
                                        && (ja.Job.Deleted == false || ja.Job.Deleted == null)); // Filtrer les jobs non supprimés
        }


        public async Task<TabJobApplication> AddJobApplicationAsync(TabJobApplication jobApplication)
        {
            _qualiProContext.TabJobApplications.Add(jobApplication);
            await _qualiProContext.SaveChangesAsync();
            return jobApplication;
        }

        public async Task<TabJobApplication> UpdateJobApplicationAsync(TabJobApplication jobApplication)
        {
            _qualiProContext.Entry(jobApplication).State = EntityState.Modified;
            await _qualiProContext.SaveChangesAsync();
            return jobApplication;
        }



        public async Task<bool> DeleteJobApplicationAsync(int id)
        {
            var jobApplication = await _qualiProContext.TabJobApplications.FindAsync(id);
            if (jobApplication == null)
                return false;
            jobApplication.Deleted = true;


            await _qualiProContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetApplicationCountByJobIdAsync(int jobId)
        {
            return await _qualiProContext.TabJobApplications
                                         .CountAsync(ja => ja.JobId == jobId);
        }

        //public async Task<IEnumerable<TabJobApplication>> GetJobsWithApplicationCount()
        //{
        //    return await _qualiProContext.TabJobApplications
        //        .Include(ja => ja.Job) // Assurez-vous d'inclure la navigation pour obtenir les titres des jobs
        //        .ToListAsync();
        //}

        // Repository method to fetch job applications without deleted candidates
        //public async Task<IEnumerable<TabJobApplication>> GetJobsWithApplicationCount()
        //{
        //    return await _qualiProContext.TabJobApplications
        //        .Include(ja => ja.Job) // Include Job for job titles
        //        .Include(ja => ja.Condidat) // Include Condidat for the Deleted flag
        //        .Where(ja => ja.Condidat == null || ja.Condidat.Deleted == false) // Filter out deleted candidates
        //        .ToListAsync();
        //}
        //public async Task<IEnumerable<TabJobApplication>> GetJobsWithApplicationCount()
        //{
        //    return await _qualiProContext.TabJobApplications
        //        .Include(ja => ja.Job)
        //        .Include(ja => ja.Condidat)
        //        .Where(ja => ja.Condidat == null || ja.Condidat.Deleted == false)
        //        .ToListAsync();
        //}
        //public async Task<IEnumerable<TabJobApplication>> GetJobsWithApplicationCount()
        //{
        //    return await _qualiProContext.TabJobApplications
        //        .Include(ja => ja.Job)
        //        .Include(ja => ja.Condidat)
        //        .Where(ja => ja.Deleted == false && (ja.Condidat == null || ja.Condidat.Deleted == false))
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<TabJobApplication>> GetJobsWithApplicationCount()
        {
            return await _qualiProContext.TabJobApplications
                .Include(ja => ja.Job)
                .Include(ja => ja.Condidat)
                .Where(ja => (ja.Deleted == false || ja.Deleted == null) // Filtrer les candidatures non supprimées
                          && (ja.Job.Deleted == false || ja.Job.Deleted == null) // Filtrer les jobs non supprimés
                          && (ja.Condidat == null || ja.Condidat.Deleted == false || ja.Condidat.Deleted == null)) // Filtrer les candidats non supprimés
                .ToListAsync();
        }


      


        public async Task<IEnumerable<TabJobApplication>> GetCandidatesWithScoreAboveThreshold()
        {
            return await _qualiProContext.TabJobApplications
                .Include(ja => ja.Job)
                .Include(ja => ja.Condidat)
                .Where(ja => ja.Score >= 12 // Filtrer les candidats avec un score >= 12
                          && (ja.Deleted == false || ja.Deleted == null) // Filtrer les candidatures non supprimées
                          && (ja.Job.Deleted == false || ja.Job.Deleted == null) // Filtrer les jobs non supprimés
                          && (ja.Condidat == null || ja.Condidat.Deleted == false || ja.Condidat.Deleted == null)) // Filtrer les candidats non supprimés
                .ToListAsync();
        }



        public async Task<IEnumerable<TabJobApplication>> GetJobApplicationsByJobIdAsync(int jobId)
        {
            return await _qualiProContext.TabJobApplications
                .Include(ja => ja.Condidat) // Inclure les candidats pour obtenir les noms
                .Where(ja => ja.JobId == jobId && (!ja.Deleted.HasValue || !ja.Deleted.Value))
                .ToListAsync();
        }

        public async Task<IEnumerable<TabCondidat>> GetCandidatesByJobApplicationIdAsync(int jobApplicationId)
        {
            return await _qualiProContext.TabJobApplications
                .Where(ja => ja.Id == jobApplicationId)
                .Include(ja => ja.Condidat) // Assurez-vous que la relation est correctement incluse
                .Select(ja => ja.Condidat)
                .ToListAsync();
        }
        public async Task<CondidatDto?> GetCondidatInfo(int condidatId, int jobId)
        {
            var jobApplication = await _qualiProContext.TabJobApplications
                .Include(ja => ja.Condidat)
                .FirstOrDefaultAsync(ja => ja.CondidatId == condidatId && ja.JobId == jobId);

            if (jobApplication?.Condidat == null)
                return null;

            return new CondidatDto
            {
                Id = jobApplication.Condidat.Id,
                FirstName = jobApplication.Condidat.FirstName,
                LastName = jobApplication.Condidat.LastName,
                Summary = jobApplication.Condidat.Summary,
                Country = jobApplication.Condidat.Country,
                PhoneNumber = jobApplication.Condidat.PhoneNumber,
                Birthdate = jobApplication.Condidat.Birthdate,
                ImageFileName = jobApplication.Condidat.ImageFileName,
                CvFileName = jobApplication.Condidat.CvFileName,
            };
        }

        public async Task<TabCondidat> GetCondidatById(int condidatId)
        {
            return await _qualiProContext.TabCondidats
                .Include(c => c.TabAccountCondidats)
                .FirstOrDefaultAsync(c => c.Id == condidatId);
        }

        public async Task<string?> GetCondidatEmailById(int? condidatId)
        {
            if (!condidatId.HasValue) return null; // Handle the case where condidatId is null

            var condidat = await GetCondidatById(condidatId.Value);
            var accountCondidat = condidat.TabAccountCondidats.FirstOrDefault();
            return accountCondidat?.Email;
        }
        public async Task<string?> GetCondidatNameById(int? condidatId)
        {
            if (!condidatId.HasValue) return null; // Handle the case where condidatId is null

            var condidat = await GetCondidatById(condidatId.Value);
            return condidat != null ? $"{condidat.FirstName} {condidat.LastName}" : null; // Return full name
        }



    }
}
