using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.QuizRepo
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QualiProContext _qualiProContext;

        public QuizRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
        }

        public async Task<IEnumerable<TabQuiz>> GetAllQuizzes()
        {
            return await _qualiProContext.TabQuizzes
                 .Where(p => p.Deleted == false || p.Deleted == null)
                .OrderByDescending(p => p.Id)
                .Include(q => q.TabProfileJobs)
                .Include(q => q.TabQuestions)
                .Include(q => q.TabQuizEvaluations)
                .ToListAsync();
        }

        public async Task<TabQuiz?> GetQuizById(int id)
        {
            return await _qualiProContext.TabQuizzes
                 .Where(p => p.Id == id && (p.Deleted == false || p.Deleted == null))

                .Include(q => q.TabProfileJobs)
                .Include(q => q.TabQuestions)
                .Include(q => q.TabQuizEvaluations)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<TabQuiz> AddQuiz(TabQuiz quiz)
        {
            _qualiProContext.TabQuizzes.Add(quiz);
            await _qualiProContext.SaveChangesAsync();
            return quiz;
        }

        public async Task<TabQuiz?> UpdateQuiz(int id, TabQuiz quiz)
        {
            var existingQuiz = await _qualiProContext.TabQuizzes
                .Include(q => q.TabProfileJobs)
                .Include(q => q.TabQuestions)
                .Include(q => q.TabQuizEvaluations)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (existingQuiz == null) return null;

            existingQuiz.QuizName = quiz.QuizName;

            // Update relationships if needed
            // e.g., existingQuiz.TabProfileJobs = quiz.TabProfileJobs;

            await _qualiProContext.SaveChangesAsync();
            return existingQuiz;
        }

        public async Task<bool> DeleteQuiz(int id)
        {
            var quiz = await _qualiProContext.TabQuizzes.FindAsync(id);
            if (quiz == null)
            {
                return false;
                quiz.Deleted = true;
            }

            await _qualiProContext.SaveChangesAsync();
            return true;
        }

        //public async Task<IEnumerable<TabQuiz>> GetQuizzesByJobApplication(int jobApplicationId)
        //{
        //    return await _qualiProContext.TabQuizzes
        //        .Where(q => q.TabProfileJobs.Any(pj => pj.TabJobs.Any(j => j.TabJobApplications.Any(ja => ja.Id == jobApplicationId))))
        //        .Where(q => q.Deleted == false || q.Deleted == null)
        //        .Include(q => q.TabProfileJobs)
        //        .Include(q => q.TabQuestions)
        //        .Include(q => q.TabQuizEvaluations)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<TabQuiz>> GetQuizzesByJobApplication(int jobApplicationId)
        {
            return await _qualiProContext.TabQuizzes
                .Where(q => q.TabProfileJobs.Any(pj => pj.TabJobs.Any(j => j.TabJobApplications.Any(ja => ja.Id == jobApplicationId))))
                .Where(q => q.Deleted == false || q.Deleted == null)
                .Include(q => q.TabProfileJobs)
                .Include(q => q.TabQuestions)
                    .ThenInclude(tq => tq.TabQuestionOptions) 
                .Include(q => q.TabQuizEvaluations)
                .ToListAsync();
        }



    }
}
