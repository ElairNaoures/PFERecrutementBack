using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.ModuleRepo
{
    public class ModuleRepository : IModuleRepository
    {

        private readonly QualiProContext _qualiProContext;

        public ModuleRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }

        public async Task<TabModule> AddModule(TabModule moduleInput)
        {
            TabModule module = new TabModule()
            {

                ModuleName = moduleInput.ModuleName,
               
            };

            await _qualiProContext.AddAsync(module);
            await _qualiProContext.SaveChangesAsync();
            return module;
        }

        public async Task<List<TabModule>> GetAllModules()
        {
            var ListModules = await _qualiProContext.TabModules.ToListAsync();
            return ListModules;
        }

        public async Task<TabModule> GetModuleById(int moduleId)
        {
            var Module = await _qualiProContext.TabModules.FirstOrDefaultAsync(p => p.Id == moduleId);
            return Module;
        }
        public async Task<TabModule> UpdateModule(int moduleId, TabModule moduleInput)
        {
            var module = await _qualiProContext.TabModules.FirstOrDefaultAsync(p => p.Id == moduleId);
          

            module.ModuleName = moduleInput.ModuleName;


            await _qualiProContext.SaveChangesAsync();
            return module;
        }

        public async Task<TabModule> DeleteModule(int moduleId)
        {
            var module = await _qualiProContext.TabModules.FindAsync(moduleId);
            if (module == null)
            {
                return null;
            }
            _qualiProContext.TabModules.Remove(module);
            await _qualiProContext.SaveChangesAsync();
            return module;
        }

    }
}
