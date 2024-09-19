using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.SkillRepo
{
    public class SkillRepository : ISkillRepository
    {
        private readonly QualiProContext _qualiProContext;

        public SkillRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
        }

        public async Task<IEnumerable<TabSkill>> GetAllSkills()
        {
            return await _qualiProContext.TabSkills.OrderByDescending(p => p.Id).ToListAsync();
        }

        public async Task<TabSkill?> GetSkillById(int id)
        {
            return await _qualiProContext.TabSkills.FindAsync(id);
        }

        public async Task<TabSkill> CreateSkill(TabSkill skill)
        {
            _qualiProContext.TabSkills.Add(skill);
            await _qualiProContext.SaveChangesAsync();
            return skill;
        }

        public async Task<TabSkill?> UpdateSkill(TabSkill skill)
        {
            var existingSkill = await _qualiProContext.TabSkills.FindAsync(skill.Id);
            if (existingSkill == null)
            {
                return null;
            }

            existingSkill.Name = skill.Name;
            existingSkill.TechnicalSkill = skill.TechnicalSkill;
            existingSkill.SoftSkill = skill.SoftSkill;
            existingSkill.ToolsSkill = skill.ToolsSkill;

            await _qualiProContext.SaveChangesAsync();

            return existingSkill;
        }

        public async Task<bool> DeleteSkill(int id)
        {
            var skill = await _qualiProContext.TabSkills.FindAsync(id);
            if (skill == null)
            {
                return false;
            }

            _qualiProContext.TabSkills.Remove(skill);
            await _qualiProContext.SaveChangesAsync();

            return true;
        }
    }
}
