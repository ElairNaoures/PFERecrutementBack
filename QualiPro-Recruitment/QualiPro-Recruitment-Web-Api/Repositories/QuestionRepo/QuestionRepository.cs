using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.QuestionRepo
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QualiProContext _qualiProContext;

        public QuestionRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
        }

        //public async Task<IEnumerable<TabQuestion>> GetAllQuestions()
        //{
        //    return await _qualiProContext.TabQuestions
        //        .Include(q => q.Quiz)
        //        .Include(q => q.CorrectQuestionOption)
        //        .Include(q => q.TabQuestionOptions)
        //        .Include(q => q.TabQuizEvaluations)
        //        .Include(q => q.TabQuestionOptions) // Inclure les options

        //        .ToListAsync();
        //}
        public async Task<IEnumerable<TabQuestion>> GetAllQuestions()
        {
            return await _qualiProContext.TabQuestions
                .Where(q => q.Deleted == false || q.Deleted == null)
                .OrderByDescending(p => p.Id) 
                .Include(q => q.Quiz) 
                .Include(q => q.CorrectQuestionOption)
                .Include(q => q.TabQuestionOptions)
                .Include(q => q.TabQuizEvaluations)
                .ToListAsync(); 
        }

        public async Task<TabQuestion?> GetQuestionById(int id)
        {
            return await _qualiProContext.TabQuestions
                .Where(q => q.Id == id && (q.Deleted == false || q.Deleted == null))

                .Include(q => q.Quiz)
                .Include(q => q.CorrectQuestionOption)
                .Include(q => q.TabQuestionOptions)
                .Include(q => q.TabQuizEvaluations)
               .Include(q => q.TabQuestionOptions) 
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<TabQuestion> AddQuestion(TabQuestion question)
        {
            question.Deleted = false;
            _qualiProContext.TabQuestions.Add(question);
            await _qualiProContext.SaveChangesAsync();
            return question;
        }

        //public async Task<TabQuestion?> UpdateQuestion(int id, TabQuestion question)
        //{
        //    var existingQuestion = await _qualiProContext.TabQuestions.FindAsync(id);
        //    if (existingQuestion == null) return null;

        //    existingQuestion.QuestionName = question.QuestionName;
        //    existingQuestion.QuizId = question.QuizId;
        //    existingQuestion.Coefficient = question.Coefficient;
        //    existingQuestion.CorrectQuestionOptionId = question.CorrectQuestionOptionId;

        //    await _qualiProContext.SaveChangesAsync();
        //    return existingQuestion;
        //}
        public async Task<TabQuestion?> UpdateQuestion(int id, TabQuestion question)
        {
            var existingQuestion = await _qualiProContext.TabQuestions.FindAsync(id);
            if (existingQuestion == null || existingQuestion.Deleted == true) return null;

            // Update properties
            existingQuestion.QuestionName = question.QuestionName;
            existingQuestion.QuizId = question.QuizId;
            existingQuestion.Coefficient = question.Coefficient;
            existingQuestion.CorrectQuestionOptionId = question.CorrectQuestionOptionId;

            await _qualiProContext.SaveChangesAsync();
            return existingQuestion;
        }

        public async Task<bool> DeleteQuestion(int id)
        {
            var question = await _qualiProContext.TabQuestions.FindAsync(id);
            if (question == null) return false;

            // Mark the question as deleted
            question.Deleted = true;

            await _qualiProContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<TabQuestion>> GetQuestionsByQuiz(int quizId)
        {
            // Only retrieve questions related to a specific quiz that are not marked as deleted
            return await _qualiProContext.TabQuestions
                .Where(q => q.QuizId == quizId && (q.Deleted == false || q.Deleted == null))
                .Include(q => q.TabQuestionOptions)
                .ToListAsync();
        }


    }
}
