using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.ContraTypeRepo
{
    public interface IContractTypeRepository
    {
        Task<TabContractType> AddContract(contractTypeDto contractInput);


        Task<List<TabContractType>> GetAllContracts();
        Task<TabContractType> GetContractById(int contractId);

        Task<TabContractType> UpdateContract(int contractId, TabContractType contractInput);
        Task<TabContractType> DeleteContract(int contractId);


    }
}
