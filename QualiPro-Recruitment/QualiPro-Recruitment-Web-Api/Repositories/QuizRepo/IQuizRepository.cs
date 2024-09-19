using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.QuizRepo
{
    public interface IQuizRepository
    {
        Task<IEnumerable<TabQuiz>> GetAllQuizzes();
        Task<TabQuiz?> GetQuizById(int id);
        Task<TabQuiz> AddQuiz(TabQuiz quiz);
        Task<TabQuiz?> UpdateQuiz(int id, TabQuiz quiz);
        Task<bool> DeleteQuiz(int id);
        Task<IEnumerable<TabQuiz>> GetQuizzesByJobApplication(int jobApplicationId);

    }
}
