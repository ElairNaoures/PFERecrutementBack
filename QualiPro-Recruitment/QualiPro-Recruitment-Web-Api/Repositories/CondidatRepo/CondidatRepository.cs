using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.CondidatRepo
{
    public class CondidatRepository : ICondidatRepository
    {

        private readonly QualiProContext _qualiProContext;

        public CondidatRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
            
        }


        public async Task<TabCondidat> AddCondidat(CondidatDto condidatInput)
        {
            TabCondidat condidat = new TabCondidat()
            {
                Summary = condidatInput.Summary,

                FirstName = condidatInput.FirstName,
                LastName = condidatInput.LastName,
                Country = condidatInput.Country,
                PhoneNumber = condidatInput.PhoneNumber,
                Birthdate = condidatInput.Birthdate,

            };


            await _qualiProContext.AddAsync(condidat);
            await _qualiProContext.SaveChangesAsync();
            return condidat;
        }



        public async Task<TabCondidat> UpdateCondidat(int condidatId, TabCondidat condidatInput)
        {
            var condidat = await _qualiProContext.TabCondidats.FirstOrDefaultAsync(p => p.Id == condidatId);
            condidat.Summary = condidatInput.Summary;

            condidat.FirstName = condidatInput.FirstName;
            condidat.LastName = condidatInput.LastName;
            condidat.Country = condidatInput.Country;
            condidat.PhoneNumber = condidatInput.PhoneNumber;
            condidat.Birthdate = condidatInput.Birthdate;

            await _qualiProContext.SaveChangesAsync();
            return condidat;
        }


        public async Task<List<TabCondidat>> GetAllCondidats()
        {
            var ListCondidats = await _qualiProContext.TabCondidats.ToListAsync();
            return ListCondidats;
        }

        public async Task<TabCondidat> GetCondidatById(int condidatId)
        {
            var Condidat = await _qualiProContext.TabCondidats.FirstOrDefaultAsync(p => p.Id == condidatId);
            return Condidat;
        }

        public async Task<TabCondidat> DeleteCondidat(int condidatId)
        {
            var condidat = await _qualiProContext.TabCondidats.FindAsync(condidatId);
            if (condidat == null)
            {
                return null;
            }
            _qualiProContext.TabCondidats.Remove(condidat);
            await _qualiProContext.SaveChangesAsync();
            return condidat;
        }

        //public async Task<string?> GetEmailByCondidatId(int condidatId)
        //{
        //    var account = await _qualiProContext.TabAccountCondidats
        //                                .FirstOrDefaultAsync(a => a.CondidatId == condidatId);
        //    return account?.Email;
        //}
        //public async Task<string?> GetEmailByCondidatId(int condidatId)
        //{
        //    var account = await _qualiProContext.TabAccountCondidats
        //                                        .FirstOrDefaultAsync(a => a.CondidatId == condidatId);
        //    return account?.Email;
        //}
    }
}
