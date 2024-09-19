using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.SkillRepo
{
    public interface ISkillRepository
    {
        Task<IEnumerable<TabSkill>> GetAllSkills();
        Task<TabSkill?> GetSkillById(int id);
        Task<TabSkill> CreateSkill(TabSkill skill);
        Task<TabSkill?> UpdateSkill(TabSkill skill);
        Task<bool> DeleteSkill(int id);
    }
}
