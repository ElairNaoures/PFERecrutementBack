using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.QuestionOptionRepo
{
    public class QuestionOptionRepository : IQuestionOptionRepository
    {
        private readonly QualiProContext _qualiProContext;

        public QuestionOptionRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
        }

        public async Task<IEnumerable<TabQuestionOption>> GetAllQuestionOptions()
        {
            return await _qualiProContext.TabQuestionOptions.ToListAsync();
        }

        public async Task<TabQuestionOption?> GetQuestionOptionById(int id)
        {
            return await _qualiProContext.TabQuestionOptions
                .Include(qo => qo.Question) // Inclut la relation avec TabQuestion
                .FirstOrDefaultAsync(qo => qo.Id == id);
        }

        public async Task<TabQuestionOption> AddQuestionOption(TabQuestionOption questionOption)
        {
            _qualiProContext.TabQuestionOptions.Add(questionOption);
            await _qualiProContext.SaveChangesAsync();
            return questionOption;
        }

        public async Task<TabQuestionOption?> UpdateQuestionOption(int id, TabQuestionOption questionOption)
        {
            var existingQuestionOption = await _qualiProContext.TabQuestionOptions.FindAsync(id);
            if (existingQuestionOption == null) return null;

            existingQuestionOption.QuestionOptionName = questionOption.QuestionOptionName;
            existingQuestionOption.QuestionId = questionOption.QuestionId;

            await _qualiProContext.SaveChangesAsync();
            return existingQuestionOption;
        }

        public async Task<bool> DeleteQuestionOption(int id)
        {
            var questionOption = await _qualiProContext.TabQuestionOptions.FindAsync(id);
            if (questionOption == null) return false;

            _qualiProContext.TabQuestionOptions.Remove(questionOption);
            await _qualiProContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TabQuestionOption>> GetAllQuestionOptionsByQuestionId(int questionId)
        {
            return await _qualiProContext.Set<TabQuestionOption>()
                                 .Where(qo => qo.QuestionId == questionId)
                                 .ToListAsync();
        }


    }
}
