using QualiPro_Recruitment_Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.QuestionRepo
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<TabQuestion>> GetAllQuestions();
        Task<TabQuestion?> GetQuestionById(int id);
        Task<TabQuestion> AddQuestion(TabQuestion question);
        Task<TabQuestion?> UpdateQuestion(int id, TabQuestion question);
        Task<bool> DeleteQuestion(int id);
        Task<IEnumerable<TabQuestion>> GetQuestionsByQuiz(int quizId);
    }
}
