using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.QuestionOptionRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionOptionController : ControllerBase
    {
        private readonly IQuestionOptionRepository _questionOptionRepository;

        public QuestionOptionController(IQuestionOptionRepository questionOptionRepository)
        {
            _questionOptionRepository = questionOptionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionOptionDto>>> GetAllQuestionOptions()
        {
            var questionOptions = await _questionOptionRepository.GetAllQuestionOptions();
            var questionOptionsDto = questionOptions.Select(qo => new QuestionOptionDto
            {
                Id = qo.Id,
                QuestionOptionName = qo.QuestionOptionName,
                QuestionId = qo.QuestionId
            });

            return Ok(questionOptionsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionOptionDto>> GetQuestionOptionById(int id)
        {
            var questionOption = await _questionOptionRepository.GetQuestionOptionById(id);
            if (questionOption == null) return NotFound();

            var questionOptionDto = new QuestionOptionDto
            {
                Id = questionOption.Id,
                QuestionOptionName = questionOption.QuestionOptionName,
                QuestionId = questionOption.QuestionId
            };

            return Ok(questionOptionDto);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionOptionDto>> AddQuestionOption(QuestionOptionDto questionOptionDto)
        {
            var questionOption = new TabQuestionOption
            {
                QuestionOptionName = questionOptionDto.QuestionOptionName,
                QuestionId = questionOptionDto.QuestionId
            };

            var createdQuestionOption = await _questionOptionRepository.AddQuestionOption(questionOption);

            var createdQuestionOptionDto = new QuestionOptionDto
            {
                Id = createdQuestionOption.Id,
                QuestionOptionName = createdQuestionOption.QuestionOptionName,
                QuestionId = createdQuestionOption.QuestionId
            };

            return CreatedAtAction(nameof(GetQuestionOptionById), new { id = createdQuestionOptionDto.Id }, createdQuestionOptionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestionOption(int id, QuestionOptionDto questionOptionDto)
        {
            var questionOption = new TabQuestionOption
            {
                QuestionOptionName = questionOptionDto.QuestionOptionName,
                QuestionId = questionOptionDto.QuestionId
            };

            var updatedQuestionOption = await _questionOptionRepository.UpdateQuestionOption(id, questionOption);
            if (updatedQuestionOption == null) return NotFound();

            var updatedQuestionOptionDto = new QuestionOptionDto
            {
                Id = updatedQuestionOption.Id,
                QuestionOptionName = updatedQuestionOption.QuestionOptionName,
                QuestionId = updatedQuestionOption.QuestionId
            };

            return Ok(updatedQuestionOptionDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionOption(int id)
        {
            var result = await _questionOptionRepository.DeleteQuestionOption(id);
            if (!result) return NotFound();

            return NoContent();
        }
        [HttpGet("question/{questionId}")]
        public async Task<ActionResult<IEnumerable<QuestionOptionDto>>> GetOptionsByQuestionId(int questionId)
        {
            var questionOptions = await _questionOptionRepository.GetAllQuestionOptionsByQuestionId(questionId);
            if (questionOptions == null || !questionOptions.Any())
            {
                return NotFound();
            }

            var questionOptionsDto = questionOptions.Select(qo => new QuestionOptionDto
            {
                Id = qo.Id,
                QuestionOptionName = qo.QuestionOptionName,
                QuestionId = qo.QuestionId
            });

            return Ok(questionOptionsDto);
        }

    }
}
