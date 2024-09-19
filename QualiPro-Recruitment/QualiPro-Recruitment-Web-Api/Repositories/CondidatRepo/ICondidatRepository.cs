using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.CondidatRepo
{
    public interface ICondidatRepository
    {
        Task<IEnumerable<CondidatDto>> GetAllCondidatsAsync();
        Task<CondidatDto> GetCondidatByIdAsync(int id);
        Task<CondidatDto> CreateCondidatAsync(CondidatDto condidatDto);
        Task UpdateCondidatAsync(CondidatDto condidatDto);

        Task<bool> DeleteCondidatAsync(int id);
    }
}
