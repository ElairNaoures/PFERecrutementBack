using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.QuizEvaluationRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizEvaluationController : ControllerBase
    {
        private readonly IQuizEvaluationRepository _quizEvaluationRepository;

        public QuizEvaluationController(IQuizEvaluationRepository quizEvaluationRepository)
        {
            _quizEvaluationRepository = quizEvaluationRepository;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<QuizEvaluationDto>>> GetAllQuizEvaluations()
        //{
        //    var quizEvaluations = await _quizEvaluationRepository.GetAllQuizEvaluationsAsync();
        //    var quizEvaluationDtos = quizEvaluations.Select(qe => new QuizEvaluationDto
        //    {
        //        Id = qe.Id,
        //        JobApplicationId = qe.JobApplicationId,
        //        QuizId = qe.QuizId,
        //        QuestionId = qe.QuestionId,
        //        QuestionOptionId = qe.QuestionOptionId,
        //        IdReponse = qe.IdReponse 

        //    });
        //    return Ok(quizEvaluationDtos);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizEvaluationNoteDto>>> GetAllQuizEvaluations()
        {
            var quizEvaluations = await _quizEvaluationRepository.GetAllQuizEvaluationsAsync();
            var quizEvaluationDtos = quizEvaluations.Select(qe => new QuizEvaluationNoteDto
            {
                Id = qe.Id,
                JobApplicationId = qe.JobApplicationId,
                QuizId = qe.QuizId,
                QuestionId = qe.QuestionId,
                QuestionOptionId = qe.QuestionOptionId,
                IdReponse = qe.IdReponse,
                QuestionName = qe.Question != null ? qe.Question.QuestionName : null,
                CorrectQuestionOptionName = qe.Question?.CorrectQuestionOption != null ? qe.Question.CorrectQuestionOption.QuestionOptionName : null,
                ReponseOptionName = qe.QuestionOption != null ? qe.QuestionOption.QuestionOptionName : null
            });

            return Ok(quizEvaluationDtos);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<QuizEvaluationDto>> GetQuizEvaluationById(int id)
        //{
        //    var quizEvaluation = await _quizEvaluationRepository.GetQuizEvaluationByIdAsync(id);
        //    if (quizEvaluation == null)
        //        return NotFound();

        //    var quizEvaluationDto = new QuizEvaluationDto
        //    {
        //        Id = quizEvaluation.Id,
        //        JobApplicationId = quizEvaluation.JobApplicationId,
        //        QuizId = quizEvaluation.QuizId,
        //        QuestionId = quizEvaluation.QuestionId,
        //        QuestionOptionId = quizEvaluation.QuestionOptionId,
        //        IdReponse = quizEvaluation.IdReponse // Ensure IdReponse is mapped

        //    };

        //    return Ok(quizEvaluationDto);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizEvaluationNoteDto>> GetQuizEvaluationById(int id)
        {
            var quizEvaluation = await _quizEvaluationRepository.GetQuizEvaluationByIdAsync(id);
            if (quizEvaluation == null)
                return NotFound();

            var quizEvaluationDto = new QuizEvaluationNoteDto
            {
                Id = quizEvaluation.Id,
                JobApplicationId = quizEvaluation.JobApplicationId,
                QuizId = quizEvaluation.QuizId,
                QuestionId = quizEvaluation.QuestionId,
                QuestionOptionId = quizEvaluation.QuestionOptionId,
                IdReponse = quizEvaluation.IdReponse,
                QuestionName = quizEvaluation.Question != null ? quizEvaluation.Question.QuestionName : null,
                CorrectQuestionOptionName = quizEvaluation.Question?.CorrectQuestionOption != null ? quizEvaluation.Question.CorrectQuestionOption.QuestionOptionName : null,
                ReponseOptionName = quizEvaluation.QuestionOption != null ? quizEvaluation.QuestionOption.QuestionOptionName : null
            };

            return Ok(quizEvaluationDto);
        }

        //[HttpPost]
        //public async Task<ActionResult<QuizEvaluationDto>> AddQuizEvaluation(QuizEvaluationDto quizEvaluationDto)
        //{
        //    var quizEvaluation = new TabQuizEvaluation
        //    {
        //        JobApplicationId = quizEvaluationDto.JobApplicationId,
        //        QuizId = quizEvaluationDto.QuizId,
        //        QuestionId = quizEvaluationDto.QuestionId,
        //        QuestionOptionId = quizEvaluationDto.QuestionOptionId
        //    };

        //    var createdQuizEvaluation = await _quizEvaluationRepository.AddQuizEvaluationAsync(quizEvaluation);

        //    var createdQuizEvaluationDto = new QuizEvaluationDto
        //    {
        //        Id = createdQuizEvaluation.Id,
        //        JobApplicationId = createdQuizEvaluation.JobApplicationId,
        //        QuizId = createdQuizEvaluation.QuizId,
        //        QuestionId = createdQuizEvaluation.QuestionId,
        //        QuestionOptionId = createdQuizEvaluation.QuestionOptionId
        //    };

        //    return CreatedAtAction(nameof(GetQuizEvaluationById), new { id = createdQuizEvaluationDto.Id }, createdQuizEvaluationDto);
        //}

        //[HttpPost]
        //public async Task<ActionResult<QuizEvaluationDto>> AddQuizEvaluation(QuizEvaluationDto quizEvaluationDto)
        //{
        //    var quizEvaluation = new TabQuizEvaluation
        //    {
        //        JobApplicationId = quizEvaluationDto.JobApplicationId,
        //        QuizId = quizEvaluationDto.QuizId,
        //        QuestionId = quizEvaluationDto.QuestionId,
        //        QuestionOptionId = quizEvaluationDto.QuestionOptionId,
        //        IdReponse = quizEvaluationDto.QuestionOptionId  // Ajoute la réponse sélectionnée ici
        //    };

        //    var createdQuizEvaluation = await _quizEvaluationRepository.AddQuizEvaluationAsync(quizEvaluation);

        //    var createdQuizEvaluationDto = new QuizEvaluationDto
        //    {
        //        Id = createdQuizEvaluation.Id,
        //        JobApplicationId = createdQuizEvaluation.JobApplicationId,
        //        QuizId = createdQuizEvaluation.QuizId,
        //        QuestionId = createdQuizEvaluation.QuestionId,
        //        QuestionOptionId = createdQuizEvaluation.QuestionOptionId,
        //        IdReponse = createdQuizEvaluation.IdReponse
        //    };

        //    return CreatedAtAction(nameof(GetQuizEvaluationById), new { id = createdQuizEvaluationDto.Id }, createdQuizEvaluationDto);
        //}
        [HttpPost]
        public async Task<ActionResult<QuizEvaluationDto>> AddQuizEvaluation(QuizEvaluationDto quizEvaluationDto)
        {
            var quizEvaluation = new TabQuizEvaluation
            {
                JobApplicationId = quizEvaluationDto.JobApplicationId,
                QuizId = quizEvaluationDto.QuizId,
                QuestionId = quizEvaluationDto.QuestionId,
                QuestionOptionId = quizEvaluationDto.QuestionOptionId,
                IdReponse = quizEvaluationDto.IdReponse  // Selected answer
            };

            var createdQuizEvaluation = await _quizEvaluationRepository.AddQuizEvaluationAsync(quizEvaluation);

            var createdQuizEvaluationDto = new QuizEvaluationDto
            {
                Id = createdQuizEvaluation.Id,
                JobApplicationId = createdQuizEvaluation.JobApplicationId,
                QuizId = createdQuizEvaluation.QuizId,
                QuestionId = createdQuizEvaluation.QuestionId,
                QuestionOptionId = createdQuizEvaluation.QuestionOptionId,
                IdReponse = createdQuizEvaluation.IdReponse
            };

            return CreatedAtAction(nameof(GetQuizEvaluationById), new { id = createdQuizEvaluationDto.Id }, createdQuizEvaluationDto);
        }



        //[HttpPost]
        //public async Task<ActionResult<QuizEvaluationDto>> AddQuizEvaluation(QuizEvaluationDto quizEvaluationDto)
        //{
        //    // Valider les données reçues
        //    if (quizEvaluationDto.JobApplicationId == null || quizEvaluationDto.QuizId == null ||
        //        quizEvaluationDto.QuestionId == null || quizEvaluationDto.QuestionOptionId == null)
        //    {
        //        return BadRequest("Une ou plusieurs propriétés sont manquantes.");
        //    }

        //    var quizEvaluation = new TabQuizEvaluation
        //    {
        //        JobApplicationId = quizEvaluationDto.JobApplicationId,
        //        QuizId = quizEvaluationDto.QuizId,
        //        QuestionId = quizEvaluationDto.QuestionId,
        //        QuestionOptionId = quizEvaluationDto.QuestionOptionId,
        //        IdReponse = quizEvaluationDto.IdReponse
        //    };

        //    try
        //    {
        //        var createdQuizEvaluation = await _quizEvaluationRepository.AddQuizEvaluationAsync(quizEvaluation);

        //        var createdQuizEvaluationDto = new QuizEvaluationDto
        //        {
        //            Id = createdQuizEvaluation.Id,
        //            JobApplicationId = createdQuizEvaluation.JobApplicationId,
        //            QuizId = createdQuizEvaluation.QuizId,
        //            QuestionId = createdQuizEvaluation.QuestionId,
        //            QuestionOptionId = createdQuizEvaluation.QuestionOptionId,
        //            IdReponse = createdQuizEvaluation.IdReponse
        //        };

        //        return CreatedAtAction(nameof(GetQuizEvaluationById), new { id = createdQuizEvaluationDto.Id }, createdQuizEvaluationDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Enregistrer l'erreur et retourner une réponse appropriée
        //        return StatusCode(500, $"Erreur serveur: {ex.Message}");
        //    }
        //}


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuizEvaluation(int id, QuizEvaluationDto quizEvaluationDto)
        {
            if (id != quizEvaluationDto.Id)
                return BadRequest();

            var quizEvaluation = new TabQuizEvaluation
            {
                Id = quizEvaluationDto.Id,
                JobApplicationId = quizEvaluationDto.JobApplicationId,
                QuizId = quizEvaluationDto.QuizId,
                QuestionId = quizEvaluationDto.QuestionId,
                QuestionOptionId = quizEvaluationDto.QuestionOptionId
            };

            try
            {
                await _quizEvaluationRepository.UpdateQuizEvaluationAsync(quizEvaluation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _quizEvaluationRepository.GetQuizEvaluationByIdAsync(id) == null)
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizEvaluation(int id)
        {
            var result = await _quizEvaluationRepository.DeleteQuizEvaluationAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        //[HttpGet("job-application/{jobApplicationId}")]
        //public async Task<ActionResult<IEnumerable<QuizEvaluationDto>>> GetQuizEvaluationsByJobApplicationId(int jobApplicationId)
        //{
        //    var quizEvaluations = await _quizEvaluationRepository.GetQuizEvaluationsByJobApplicationIdAsync(jobApplicationId);

        //    if (!quizEvaluations.Any())
        //    {
        //        return NotFound($"No quiz evaluations found for JobApplicationId: {jobApplicationId}");
        //    }

        //    var quizEvaluationDtos = quizEvaluations.Select(qe => new QuizEvaluationDto
        //    {
        //        Id = qe.Id,
        //        JobApplicationId = qe.JobApplicationId,
        //        QuizId = qe.QuizId,
        //        QuestionId = qe.QuestionId,
        //        QuestionOptionId = qe.QuestionOptionId,
        //        IdReponse = qe.IdReponse,
        //        QuestionName = qe.Question != null ? qe.Question.QuestionName : null,
        //        CorrectQuestionOptionName = qe.Question?.CorrectQuestionOption != null
        //    ? qe.Question.CorrectQuestionOption.QuestionOptionName : null ,
        //        ReponseOptionName = qe.QuestionOption != null
        //    ? qe.QuestionOption.QuestionOptionName
        //    : null
        //    });

        //    return Ok(quizEvaluationDtos);
        //}

        [HttpGet("job-application/{jobApplicationId}")]
        public async Task<ActionResult<IEnumerable<QuizEvaluationNoteDto>>> GetQuizEvaluationsByJobApplicationId(int jobApplicationId)
        {
            var quizEvaluations = await _quizEvaluationRepository.GetQuizEvaluationsByJobApplicationIdAsync(jobApplicationId);

            if (quizEvaluations == null || !quizEvaluations.Any())
            {
                return NotFound($"No quiz evaluations found for JobApplicationId: {jobApplicationId}");
            }

            var quizEvaluationDtos = quizEvaluations.Select(qe => new QuizEvaluationNoteDto
            {
                Id = qe.Id,
                JobApplicationId = qe.JobApplicationId,
                QuizId = qe.QuizId,
                QuestionId = qe.QuestionId,
                QuestionOptionId = qe.QuestionOptionId,
                IdReponse = qe.IdReponse,
                QuestionName = qe.Question != null ? qe.Question.QuestionName : null,
                CorrectQuestionOptionName = qe.Question?.CorrectQuestionOption != null ? qe.Question.CorrectQuestionOption.QuestionOptionName : null,
                ReponseOptionName = qe.QuestionOption != null ? qe.QuestionOption.QuestionOptionName : null,
                Coefficient = qe.Question?.Coefficient 

            });

            return Ok(quizEvaluationDtos);
        }

    }
  



}
