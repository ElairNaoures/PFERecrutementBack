using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.QuizEvaluationRepo
{
    public interface IQuizEvaluationRepository
    {
        Task<IEnumerable<TabQuizEvaluation>> GetAllQuizEvaluationsAsync();
        Task<TabQuizEvaluation> GetQuizEvaluationByIdAsync(int id);

        Task<TabQuizEvaluation> AddQuizEvaluationAsync(TabQuizEvaluation quizEvaluation);
        Task<TabQuizEvaluation> UpdateQuizEvaluationAsync(TabQuizEvaluation quizEvaluation);
        Task<bool> DeleteQuizEvaluationAsync(int id);
        // Task<IEnumerable<TabQuizEvaluation>> GetQuizEvaluationsByJobApplicationIdAsync(int jobApplicationId);
        Task<IEnumerable<TabQuizEvaluation>> GetQuizEvaluationsByJobApplicationIdAsync(int jobApplicationId);


    }
}
