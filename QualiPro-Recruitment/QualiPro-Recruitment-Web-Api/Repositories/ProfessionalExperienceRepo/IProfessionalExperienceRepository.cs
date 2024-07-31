using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.ProfessionalExperienceRepo
{
    public interface IProfessionalExperienceRepository
    {
        Task<ProfessionalExperience> AddProfessionalExperience(ProfessionalExperience professionalExperienceInput);
        Task<List<ProfessionalExperience>> GetAllProfessionalExperiences();
        Task<ProfessionalExperience> GetProfessionalExperienceById(int professionalExperienceId);
        Task<ProfessionalExperience> UpdateProfessionalExperience(int professionalExperienceId, ProfessionalExperience professionalExperienceInput);
        Task<ProfessionalExperience> DeleteProfessionalExperience(int professionalExperienceId);
       // Task<IActionResult> GetProfessionalExperiencesByCondidatId(int condidatId);
        Task<List<ProfessionalExperience>> GetProfessionalExperiencesByCondidatId(int condidatId); // Nouvelle méthode

    }
}
