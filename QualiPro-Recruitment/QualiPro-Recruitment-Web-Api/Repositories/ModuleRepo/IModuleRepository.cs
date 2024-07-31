using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.ModuleRepo
{
    public interface IModuleRepository
    {
        Task<TabModule> AddModule(TabModule moduleInput);
        Task<List<TabModule>> GetAllModules();
        Task<TabModule> GetModuleById(int moduleId);
        Task<TabModule> UpdateModule(int moduleId, TabModule moduleInput);
        Task<TabModule> DeleteModule(int moduleId);

    }
}
