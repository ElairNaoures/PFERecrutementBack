using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.RoleRepo
{
    public interface IRoleRepository
    {
        Task<TabRole> AddRole(TabRole roleInput);
        Task<List<TabRole>> GetAllRoles();
        Task<TabRole> GetRoleById(int roleId);
        Task<TabRole> UpdateRole(int roleId, TabRole roleInput);
        Task<TabRole> DeleteRole(int roleId);

    }
}
