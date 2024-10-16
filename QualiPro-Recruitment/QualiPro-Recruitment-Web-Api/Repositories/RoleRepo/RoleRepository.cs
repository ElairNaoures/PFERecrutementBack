﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.RoleRepo
{
    public class RoleRepository : IRoleRepository
    {
        private readonly QualiProContext _qualiProContext;

        public RoleRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }

        public async Task<TabRole> AddRole(TabRole roleInput)
        {
            TabRole role = new TabRole()
            {
            
                RoleName = roleInput.RoleName,

            };

            await _qualiProContext.AddAsync(role);
            await _qualiProContext.SaveChangesAsync();
            return role;
        }

        public async Task<List<TabRole>> GetAllRoles()
        {
            var ListRoles = await _qualiProContext.TabRoles.OrderByDescending(p => p.Id).Where(p => p.Deleted == false || p.Deleted == null).ToListAsync();
            return ListRoles;
        }

        public async Task<TabRole> GetRoleById(int roleId)
        {
            var Role = await _qualiProContext.TabRoles.FirstOrDefaultAsync(p => p.Id == roleId && (p.Deleted == false || p.Deleted == null));
            return Role;
        }

        public async Task<TabRole> UpdateRole(int roleId, TabRole roleInput)
        {
            var role = await _qualiProContext.TabRoles.FirstOrDefaultAsync(p => p.Id == roleId);



            role.RoleName = roleInput.RoleName;

            await _qualiProContext.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            var role = await _qualiProContext.TabRoles.FindAsync(roleId);
            if (role == null)
                return false;
            role.Deleted = true;
            await _qualiProContext.SaveChangesAsync();
            return true;
        }

    }
}
