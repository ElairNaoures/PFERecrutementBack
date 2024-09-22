using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.QuizRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAllQuizzes()
        {
            var quizzes = await _quizRepository.GetAllQuizzes();
            var quizDtos = quizzes.Select(q => new QuizDto
            {
                Id = q.Id,
                QuizName = q.QuizName,
                JobProfileId = q.TabProfileJobs.FirstOrDefault()?.Id ?? 0,
                QuestionId = q.TabQuestions.FirstOrDefault()?.Id ?? 0,
                QuizEvaluationId = q.TabQuizEvaluations.FirstOrDefault()?.Id ?? 0
            });

            return Ok(quizDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDto>> GetQuizById(int id)
        {
            var quiz = await _quizRepository.GetQuizById(id);
            if (quiz == null) return NotFound();

            var quizDto = new QuizDto
            {
                Id = quiz.Id,
                QuizName = quiz.QuizName,
                JobProfileId = quiz.TabProfileJobs.FirstOrDefault()?.Id ?? 0,
                QuestionId = quiz.TabQuestions.FirstOrDefault()?.Id ?? 0,
                QuizEvaluationId = quiz.TabQuizEvaluations.FirstOrDefault()?.Id ?? 0
            };

            return Ok(quizDto);
        }

        [HttpPost]
        public async Task<ActionResult<QuizDto>> AddQuiz(QuizDto quizDto)
        {
            var quiz = new TabQuiz
            {
                QuizName = quizDto.QuizName,
                // Populate related entities if necessary
            };

            var createdQuiz = await _quizRepository.AddQuiz(quiz);

            quizDto.Id = createdQuiz.Id;
            return CreatedAtAction(nameof(GetQuizById), new { id = quizDto.Id }, quizDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, QuizDto quizDto)
        {
            var quiz = new TabQuiz
            {
                Id = id,
                QuizName = quizDto.QuizName,
                // Update related entities if necessary
            };

            var updatedQuiz = await _quizRepository.UpdateQuiz(id, quiz);
            if (updatedQuiz == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var result = await _quizRepository.DeleteQuiz(id);
            if (!result) return NotFound();
            return NoContent();
        }

        //[HttpGet("job-application/{jobApplicationId}")]
        //public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzesByJobApplication(int jobApplicationId)
        //{
        //    var quizzes = await _quizRepository.GetQuizzesByJobApplication(jobApplicationId);
        //    if (quizzes == null || !quizzes.Any()) return NotFound();

        //    var quizDtos = quizzes.Select(q => new QuizDto
        //    {
        //        Id = q.Id,
        //        QuizName = q.QuizName,
        //        JobProfileId = q.TabProfileJobs.FirstOrDefault()?.Id ?? 0,
        //        Questions = q.TabQuestions.Select(tq => new QuestionDto
        //        {
        //            Id = tq.Id,
        //            QuestionName = tq.QuestionName,
        //            QuizId = tq.QuizId,
        //            Coefficient = tq.Coefficient,
        //            CorrectQuestionOptionId = tq.CorrectQuestionOptionId,
        //            QuestionOptions = tq.TabQuestionOptions.Select(qo => new QuestionOptionDto
        //            {
        //                Id = qo.Id,
        //                QuestionOptionName = qo.QuestionOptionName,
        //                QuestionId = qo.QuestionId
        //            }).ToList()
        //        }).ToList()
        //    });

        //    return Ok(quizDtos);
        //}

        [HttpGet("job-application/{jobApplicationId}")]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzesByJobApplication(int jobApplicationId)
        {
            var quizzes = await _quizRepository.GetQuizzesByJobApplication(jobApplicationId);
            if (quizzes == null || !quizzes.Any()) return NotFound();

            var quizDtos = quizzes.Select(q => new QuizDto
            {
                Id = q.Id,
                QuizName = q.QuizName,
                JobProfileId = q.TabProfileJobs.FirstOrDefault()?.Id ?? 0,
                Questions = q.TabQuestions
                    // Corrigez la conversion en bool pour "Deleted"
                    .Where(tq => tq.Deleted == false || tq.Deleted == null)
                    .Select(tq => new QuestionDto
                    {
                        Id = tq.Id,
                        QuestionName = tq.QuestionName,
                        QuizId = tq.QuizId,
                        Coefficient = tq.Coefficient,
                        CorrectQuestionOptionId = tq.CorrectQuestionOptionId,
                        QuestionOptions = tq.TabQuestionOptions.Select(qo => new QuestionOptionDto
                        {
                            Id = qo.Id,
                            QuestionOptionName = qo.QuestionOptionName,
                            QuestionId = qo.QuestionId
                        }).ToList()
                    }).ToList()
            });

            return Ok(quizDtos);
        }


    }
}
