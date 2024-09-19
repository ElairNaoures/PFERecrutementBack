using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.QuizEvaluationRepo
{
    public class QuizEvaluationRepository : IQuizEvaluationRepository
    {
        private readonly QualiProContext _qualiProContext;

        public QuizEvaluationRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
        }

        //public async Task<IEnumerable<TabQuizEvaluation>> GetAllQuizEvaluationsAsync()
        //{
        //    return await _qualiProContext.TabQuizEvaluations.OrderByDescending(p => p.Id).ToListAsync();
        //}
        public async Task<IEnumerable<TabQuizEvaluation>> GetAllQuizEvaluationsAsync()
        {
            return await _qualiProContext.TabQuizEvaluations
                .Include(e => e.Question) // Inclure les questions
                .Include(e => e.Question.CorrectQuestionOption) // Inclure l'option correcte de la question
                .Include(e => e.QuestionOption) // Inclure l'option sélectionnée
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }
        //public async Task<TabQuizEvaluation> GetQuizEvaluationByIdAsync(int id)
        //{
        //    return await _qualiProContext.TabQuizEvaluations.FindAsync(id);
        //}

        public async Task<TabQuizEvaluation> GetQuizEvaluationByIdAsync(int id)
        {
            return await _qualiProContext.TabQuizEvaluations
                .Include(e => e.Question) // Inclure les questions
                .Include(e => e.Question.CorrectQuestionOption) // Inclure l'option correcte de la question
                .Include(e => e.QuestionOption) // Inclure l'option sélectionnée
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TabQuizEvaluation> AddQuizEvaluationAsync(TabQuizEvaluation quizEvaluation)
        {
            _qualiProContext.TabQuizEvaluations.Add(quizEvaluation);
            await _qualiProContext.SaveChangesAsync();
            return quizEvaluation;
        }

        public async Task<TabQuizEvaluation> UpdateQuizEvaluationAsync(TabQuizEvaluation quizEvaluation)
        {
            _qualiProContext.Entry(quizEvaluation).State = EntityState.Modified;
            await _qualiProContext.SaveChangesAsync();
            return quizEvaluation;
        }

        public async Task<bool> DeleteQuizEvaluationAsync(int id)
        {
            var quizEvaluation = await _qualiProContext.TabQuizEvaluations.FindAsync(id);
            if (quizEvaluation == null)
                return false;

            _qualiProContext.TabQuizEvaluations.Remove(quizEvaluation);
            await _qualiProContext.SaveChangesAsync();
            return true;
        }
        //public async Task<IEnumerable<TabQuizEvaluation>> GetQuizEvaluationsByJobApplicationIdAsync(int jobApplicationId)
        //{
        //    return await _qualiProContext.TabQuizEvaluations
        //       .Where(e => e.JobApplicationId == jobApplicationId)
        //       .Include(e => e.Question) // Incluez les questions
        //       .Include(e => e.Question.CorrectQuestionOption)
        //        .Include(e => e.QuestionOption)
        //       .ToListAsync();
        //}

        public async Task<IEnumerable<TabQuizEvaluation>> GetQuizEvaluationsByJobApplicationIdAsync(int jobApplicationId)
        {
            return await _qualiProContext.TabQuizEvaluations
                .Include(qe => qe.Question)
                .ThenInclude(q => q.CorrectQuestionOption)
                .Include(qe => qe.QuestionOption)
                .Where(qe => qe.JobApplicationId == jobApplicationId)
                .ToListAsync();
        }

    }
}
