using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.Repositories.CompteRepo;


namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICompteRepository _compteRepository;

        public AccountController(ICompteRepository compteRepository)
        {
            _compteRepository = compteRepository;
        }


        [HttpPost]
        public async Task<IActionResult> AddCompte([FromBody] TabAccount compteInput)
        {
            var Compte = await _compteRepository.AddCompte(compteInput);
            return Ok(Compte);


        }

        [HttpGet]
        public async Task<IActionResult> GetAllComptes()
        {
            var ListComptes = await _compteRepository.GetAllComptes();
            return Ok(ListComptes);

        }
        [HttpGet("{compteId}")]
        public async Task<IActionResult> GetCompteId(int compteId)
        {
            var Compte = await _compteRepository.GetCompteById(compteId);
            return Ok(Compte);


        }


        [HttpPut("{compteId}")]
        public async Task<IActionResult> UpdateCompte([FromRoute] int compteId, [FromBody] TabAccount compteInput)
        {
           
            var Compte = await _compteRepository.UpdateCompte(compteId, compteInput);
            return Ok(Compte);

        }

        [HttpDelete("{compteId}")]
        public async Task<IActionResult> DeleteCompte(int compteId)
        {
            var deletedCompte = await _compteRepository.DeleteCompte(compteId);
            if (deletedCompte == null)
            {
                return NotFound("Compte not found");
            }
            return Ok("Compte deleted successfully");
        }

    }
}
