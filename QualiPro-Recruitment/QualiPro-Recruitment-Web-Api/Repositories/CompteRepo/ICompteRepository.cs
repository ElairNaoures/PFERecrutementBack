using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.CompteRepo
{
    public interface ICompteRepository
    {
        Task<TabAccount> AddCompte(TabAccount compteInput);
        Task<List<TabAccount>> GetAllComptes();

        Task<TabAccount> GetCompteById(int compteId);
        Task<TabAccount> UpdateCompte(int compteId, TabAccount compteInput);
        Task<TabAccount> DeleteCompte(int compteId);



    }


}
