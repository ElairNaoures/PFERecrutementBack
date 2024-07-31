using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.EducationRepo
{
    public class EducationRepository : IEducationRepository
    {
        private readonly QualiProContext _qualiProContext;

        public EducationRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }
        public async Task<TabEducation> AddEducation(EducationDto educationInput)
        {
            TabEducation education = new TabEducation()
            {

                Title = educationInput.Title,
                Establishment = educationInput.Establishment,
                StartDate = educationInput.StartDate,
                EndDate = educationInput.EndDate,
                Description = educationInput.Description,
                CondidatId = educationInput.CondidatId,


        };


            await _qualiProContext.AddAsync(education);
            await _qualiProContext.SaveChangesAsync();
            return education;
        }


       


        public async Task<List<TabEducation>> GetAllEducations()
        {
            var ListEducations = await _qualiProContext.TabEducations.ToListAsync();
            return ListEducations;
        }

        public async Task<TabEducation> GetEducationById(int educationId)
        {
            var Education = await _qualiProContext.TabEducations.FirstOrDefaultAsync(p => p.Id == educationId);
            return Education;
        }


        public async Task<TabEducation> UpdateEducation(int educationId, TabEducation educationInput)
        {
            var education = await _qualiProContext.TabEducations.FirstOrDefaultAsync(p => p.Id == educationId);

            education.Title = educationInput.Title;
            education.Establishment = educationInput.Establishment;
            education.StartDate = educationInput.StartDate;
                education.EndDate = educationInput.EndDate;
            education.Description = educationInput.Description;
            education.CondidatId = educationInput.CondidatId ;


            await _qualiProContext.SaveChangesAsync();
            return education;
        }

        public async Task<TabEducation> DeleteEducation(int educationId)
        {
            var education = await _qualiProContext.TabEducations.FindAsync(educationId);
            if (education == null)
            {
                return null;
            }
            _qualiProContext.TabEducations.Remove(education);
            await _qualiProContext.SaveChangesAsync();
            return education;
        }
        public async Task<List<TabEducation>> GetEducationsByCondidatId(int condidatId)
        {
            return await _qualiProContext.TabEducations
                                         .Where(e => e.CondidatId == condidatId)
                                         .ToListAsync();
        }
    }
}

   
