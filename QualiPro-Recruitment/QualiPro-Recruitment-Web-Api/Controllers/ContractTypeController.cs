using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.ContraTypeRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractTypeController : ControllerBase
    {

        private readonly IContractTypeRepository _contractTypeRepository;

        public ContractTypeController(IContractTypeRepository contractTypeRepository)
        {
            _contractTypeRepository = contractTypeRepository;
        }

        //[HttpPost]
        //public async Task<IActionResult> AddContract([FromBody] TabContractType contractInput)
        //{
        //    var Contract = await _contractTypeRepository.AddContract(contractInput);
        //    return Ok(Contract);


        //}

        [HttpPost]
        public async Task<IActionResult> AddContract([FromBody] contractTypeDto contractInput)
        {

            TabContractType Contract = await _contractTypeRepository.AddContract(contractInput);
            return Ok(Contract);
        }

       

        [HttpGet]
        public async Task<IActionResult> GetAllContracts()
        {
            var ListContracts = await _contractTypeRepository.GetAllContracts();
            return Ok(ListContracts);

        }

        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetContractId(int contractId)
        {
            var Contract = await _contractTypeRepository.GetContractById(contractId);
            return Ok(Contract);


        }

        [HttpPut("{contractId}")]
        public async Task<IActionResult> UpdateContract([FromRoute] int contractId, [FromBody] TabContractType contractInput)
        {
            var Contract = await _contractTypeRepository.UpdateContract(contractId, contractInput);
            return Ok(Contract);


        }


        [HttpDelete("{contractId}")]
        public async Task<IActionResult> DeleteContract(int contractId)
        {
            var deletedContract = await _contractTypeRepository.DeleteContract(contractId);
            if (deletedContract == null)
            {
                return NotFound("Contract not found");
            }
            return Ok("Contract deleted successfully");
        }
    }
}
