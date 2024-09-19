using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.ContraTypeRepo
{
    public class ContractTypeRepository : IContractTypeRepository
    {
        private readonly QualiProContext _qualiProContext;

        public ContractTypeRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }
        public async Task<TabContractType> AddContract(contractTypeDto contractInput)
        {
            TabContractType contract = new TabContractType()
            {

                Designation = contractInput.Designation,
                CreatedAt = DateTime.Now
            };

            await _qualiProContext.AddAsync(contract);
            await _qualiProContext.SaveChangesAsync();
            return contract;
        }

      

        public async Task<List<TabContractType>> GetAllContracts()
        {
            var ListContracts = await _qualiProContext.TabContractTypes.OrderByDescending(p => p.Id).Where(p => p.Deleted == false || p.Deleted == null).ToListAsync();
            return ListContracts;
        }

        public async Task<TabContractType> GetContractById(int contractId)
        {
            var Contract = await _qualiProContext.TabContractTypes
                
                .FirstOrDefaultAsync(p => p.Id == contractId && (p.Deleted == false || p.Deleted == null));
            return Contract;
        }

        public async Task<TabContractType> UpdateContract(int contractId, TabContractType contractInput)
        {
            var contract = await _qualiProContext.TabContractTypes.FirstOrDefaultAsync(p => p.Id == contractId);


            contract.Designation = contractInput.Designation;
            contract.CreatedAt = contractInput.CreatedAt;

        await _qualiProContext.SaveChangesAsync();
            return contract;
        }

        public async Task<bool> DeleteContract(int contractId)
        {
            var contract = await _qualiProContext.TabContractTypes.FindAsync(contractId);
            if (contract == null)
            {
                return false;
                contract.Deleted = true;
            }
            await _qualiProContext.SaveChangesAsync();
            return true;
        }
       
    }
}
