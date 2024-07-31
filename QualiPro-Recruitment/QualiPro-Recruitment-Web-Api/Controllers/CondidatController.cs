using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Models;
using QualiPro_Recruitment_Web_Api.Repositories.AuthRepo;
using QualiPro_Recruitment_Web_Api.Repositories.CompteRepo;
using QualiPro_Recruitment_Web_Api.Repositories.CondidatRepo;
using QualiPro_Recruitment_Web_Api.Repositories.JobRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CondidatController : ControllerBase
    {

        private readonly ICondidatRepository _condidatRepository;
        private readonly IAuthRepository _authRepository;



        public CondidatController(ICondidatRepository condidatRepository, IAuthRepository authRepository)
        {
            _condidatRepository = condidatRepository;
            _authRepository = authRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllCondidats()
        {
            var ListCondidats = await _condidatRepository.GetAllCondidats();
            return Ok(ListCondidats);

        }
        [HttpGet("{condidatId}")]
        public async Task<IActionResult> GetCondidatId(int condidatId)
        {
            var Condidat = await _condidatRepository.GetCondidatById(condidatId);
            return Ok(Condidat);


        }

        [HttpPost]
        public async Task<IActionResult> AddCondidat([FromBody] CondidatDto condidatInput)
        {

            TabCondidat Condidat = await _condidatRepository.AddCondidat(condidatInput);
            return Ok(Condidat);
        }

        [HttpPut("{condidatId}")]
        public async Task<IActionResult> UpdateCondidat([FromRoute] int condidatId, [FromBody] TabCondidat condidatInput)
        {
            var Condidat = await _condidatRepository.UpdateCondidat(condidatId, condidatInput);
            return Ok(Condidat);


        }


        [HttpDelete("{condidatId}")]
        public async Task<IActionResult> DeleteCondidat(int condidatId)
        {
            var deletedCondidat = await _condidatRepository.DeleteCondidat(condidatId);
            if (deletedCondidat == null)
            {
                return NotFound("condidat not found");
            }
            return Ok("condidat deleted successfully");
        }

        //[HttpGet("{condidatId}/email")]
        //public async Task<IActionResult> GetEmailByCondidatId(int condidatId)
        //{
        //    var email = await _authRepository.GetEmailByCondidatId(condidatId);
        //    if (email == null)
        //    {
        //        return NotFound("Email not found.");
        //    }
        //    return Ok(new { Email = email });
        //}

    } 
}