using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.QuestionOptionRepo;
using QualiPro_Recruitment_Web_Api.Repositories.QuestionRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionOptionRepository _questionOptionRepository;


        public QuestionController(IQuestionRepository questionRepository, IQuestionOptionRepository questionOptionRepository)
        {
            _questionRepository = questionRepository;
            _questionOptionRepository = questionOptionRepository;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllQuestions()
        {
            var questions = await _questionRepository.GetAllQuestions();
            var questionDtos = new List<QuestionDto>();

            foreach (var question in questions)
            {
                var questionOptions = await _questionOptionRepository.GetAllQuestionOptionsByQuestionId(question.Id);
                questionDtos.Add(new QuestionDto
                {
                    Id = question.Id,
                    QuestionName = question.QuestionName,
                    QuizId = question.QuizId,
                    Coefficient = question.Coefficient,
                    CorrectQuestionOptionId = question.CorrectQuestionOptionId,
                    QuestionOptions = questionOptions.Select(qo => new QuestionOptionDto
                    {
                        Id = qo.Id,
                        QuestionOptionName = qo.QuestionOptionName,
                        QuestionId = qo.QuestionId
                    }).ToList()
                });
            }

            return Ok(questionDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(int id)
        {
            var question = await _questionRepository.GetQuestionById(id);
            if (question == null) return NotFound();

            var questionOptions = await _questionOptionRepository.GetAllQuestionOptionsByQuestionId(id);
            var questionDto = new QuestionDto
            {
                Id = question.Id,
                QuestionName = question.QuestionName,
                QuizId = question.QuizId,
                Coefficient = question.Coefficient,
                CorrectQuestionOptionId = question.CorrectQuestionOptionId,
                QuestionOptions = questionOptions.Select(qo => new QuestionOptionDto
                {
                    Id = qo.Id,
                    QuestionOptionName = qo.QuestionOptionName,
                    QuestionId = qo.QuestionId
                }).ToList()
            };

            return Ok(questionDto);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> AddQuestion(QuestionDto questionDto)
        {
            var question = new TabQuestion
            {
                QuestionName = questionDto.QuestionName,
                QuizId = questionDto.QuizId,
                Coefficient = questionDto.Coefficient,
               CorrectQuestionOptionId = questionDto.CorrectQuestionOptionId
            };

            var createdQuestion = await _questionRepository.AddQuestion(question);

            questionDto.Id = createdQuestion.Id;
            return CreatedAtAction(nameof(GetQuestionById), new { id = questionDto.Id }, questionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, QuestionDto questionDto)
        {
            var question = new TabQuestion
            {
                Id = id,
                QuestionName = questionDto.QuestionName,
                QuizId = questionDto.QuizId,
                Coefficient = questionDto.Coefficient,
                CorrectQuestionOptionId = questionDto.CorrectQuestionOptionId
            };

            var updatedQuestion = await _questionRepository.UpdateQuestion(id, question);
            if (updatedQuestion == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var result = await _questionRepository.DeleteQuestion(id);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsByQuiz(int quizId)
        {
            var questions = await _questionRepository.GetQuestionsByQuiz(quizId);
            if (questions == null || !questions.Any()) return NotFound();

            var questionDtos = questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                QuestionName = q.QuestionName,
                QuizId = q.QuizId,
                Coefficient = q.Coefficient,
                CorrectQuestionOptionId = q.CorrectQuestionOptionId,
                QuestionOptions = q.TabQuestionOptions.Select(qo => new QuestionOptionDto
                {
                    Id = qo.Id,
                    QuestionOptionName = qo.QuestionOptionName,
                    QuestionId = qo.QuestionId
                }).ToList()
            }).ToList();

            return Ok(questionDtos);
        }

    }
}
