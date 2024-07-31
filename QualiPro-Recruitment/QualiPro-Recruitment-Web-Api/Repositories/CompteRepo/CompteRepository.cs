using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.CompteRepo
{
    public class CompteRepository : ICompteRepository
    {
        private readonly QualiProContext _qualiProContext;

        public CompteRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }

        public async Task<TabAccount> AddCompte(TabAccount compteInput)
        {
            TabAccount compte = new TabAccount()
            {
                Email = compteInput.Email,
                Password = compteInput.Password,
                Blocked = compteInput.Blocked

            };

            await _qualiProContext.AddAsync(compte);
            await _qualiProContext.SaveChangesAsync();
            return compte;
        }


        public async Task<List<TabAccount>> GetAllComptes()
        {
            var ListComptes = await _qualiProContext.TabAccounts.ToListAsync();
            return ListComptes;
        }

        public async Task<TabAccount> GetCompteById(int compteId)
        {
            var Compte = await _qualiProContext.TabAccounts.FirstOrDefaultAsync(p => p.Id == compteId);
            return Compte;
        }


        public async Task<TabAccount> UpdateCompte(int compteId, TabAccount compteInput)
        {
            var compte = await _qualiProContext.TabAccounts.FirstOrDefaultAsync(p => p.Id == compteId);
            

            compte.Email = compteInput.Email;
            compte.Password = compteInput.Password;

            await _qualiProContext.SaveChangesAsync();
            return compte;
        }

        public async Task<TabAccount> DeleteCompte(int compteId)
        {
            var compte = await _qualiProContext.TabAccounts.FindAsync(compteId);
            if (compte == null)
            {
                return null;
            }
            _qualiProContext.TabAccounts.Remove(compte);
            await _qualiProContext.SaveChangesAsync();
            return compte;
        }
        //public async Task<TabAccountCondidat?> GetAccountByCondidatId(int condidatId)
        //{
        //    return await _qualiProContext.TabAccountCondidats
        //        .FirstOrDefaultAsync(a => a.CondidatId == condidatId);
        //}
    }
}
