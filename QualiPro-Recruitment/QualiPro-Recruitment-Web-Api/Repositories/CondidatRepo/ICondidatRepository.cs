using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.CondidatRepo
{
    public interface ICondidatRepository
    {
        Task<TabCondidat> AddCondidat(CondidatDto condidatInput);
        Task<TabCondidat> UpdateCondidat(int condidatId, TabCondidat condidatInput);

        Task<TabCondidat> GetCondidatById(int condidatId);
        Task<List<TabCondidat>> GetAllCondidats();
        Task<TabCondidat> DeleteCondidat(int condidatId);
        //Task<string?> GetEmailByCondidatId(int condidatId);

    }
}
