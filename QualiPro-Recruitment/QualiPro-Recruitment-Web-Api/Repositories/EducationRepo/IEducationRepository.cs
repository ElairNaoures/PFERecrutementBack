using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.EducationRepo
{
    public interface IEducationRepository
    {
        Task<TabEducation> AddEducation(EducationDto educationInput);

       // Task<IActionResult> AddEducation([FromBody] EducationDto educationInput);
        Task<List<TabEducation>> GetAllEducations();
        Task<TabEducation> GetEducationById(int educationId);
        Task<TabEducation> UpdateEducation(int educationId, TabEducation educationInput);
        Task<TabEducation> DeleteEducation(int educationId);
        Task<List<TabEducation>> GetEducationsByCondidatId(int condidatId); 

    }
}
