using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QualiPro_Recruitment_Web_Api.Repositories.ProfessionalExperienceRepo
{
    public class ProfessionalExperienceRepository : IProfessionalExperienceRepository
    {
        private readonly QualiProContext _qualiProContext;

        public ProfessionalExperienceRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }

        public async Task<ProfessionalExperience> AddProfessionalExperience(ProfessionalExperience professionalExperienceInput)
        {
            ProfessionalExperience professionalExperience = new ProfessionalExperience()
            {
                Title = professionalExperienceInput.Title,
                Company = professionalExperienceInput.Company,
                Location = professionalExperienceInput.Location,
                StartDate = professionalExperienceInput.StartDate,
                EndDate = professionalExperienceInput.EndDate,
                Description = professionalExperienceInput.Description,
                CondidatId = professionalExperienceInput.CondidatId


         };

            await _qualiProContext.AddAsync(professionalExperience);
            await _qualiProContext.SaveChangesAsync();
            return professionalExperience;
        }

        public async Task<List<ProfessionalExperience>> GetAllProfessionalExperiences()
        {
            var ListProfessionalExperiences = await _qualiProContext.ProfessionalExperiences.ToListAsync();
            return ListProfessionalExperiences;
        }

        public async Task<ProfessionalExperience> GetProfessionalExperienceById(int professionalExperienceId)
        {
            var ProfessionalExperience = await _qualiProContext.ProfessionalExperiences.FirstOrDefaultAsync(p => p.Id == professionalExperienceId);
            return ProfessionalExperience;
        }

        public async Task<ProfessionalExperience> UpdateProfessionalExperience(int professionalExperienceId, ProfessionalExperience professionalExperienceInput)
        {
            var professionalExperience = await _qualiProContext.ProfessionalExperiences.FirstOrDefaultAsync(p => p.Id == professionalExperienceId);


            professionalExperience.Title = professionalExperienceInput.Title;
            professionalExperience.Company = professionalExperienceInput.Company;
            professionalExperience.Location = professionalExperienceInput.Location;
            professionalExperience.StartDate = professionalExperienceInput.StartDate;
            professionalExperience.EndDate = professionalExperienceInput.EndDate;
            professionalExperience.Description = professionalExperienceInput.Description;
            professionalExperience.CondidatId = professionalExperienceInput.CondidatId;


            

            await _qualiProContext.SaveChangesAsync();
            return professionalExperience;
        }

        public async Task<ProfessionalExperience> DeleteProfessionalExperience(int professionalExperienceId)
        {
            var professionalExperience = await _qualiProContext.ProfessionalExperiences.FindAsync(professionalExperienceId);
            if (professionalExperience == null)
            {
                return null;
            }
            _qualiProContext.ProfessionalExperiences.Remove(professionalExperience);
            await _qualiProContext.SaveChangesAsync();
            return professionalExperience;
        }

        public async Task<List<ProfessionalExperience>> GetProfessionalExperiencesByCondidatId(int condidatId)
        {
            return await _qualiProContext.ProfessionalExperiences
                                         .Where(p => p.CondidatId == condidatId)
                                         .ToListAsync();
        }
    }
}
