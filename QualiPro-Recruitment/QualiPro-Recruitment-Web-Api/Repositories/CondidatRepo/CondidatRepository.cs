using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Models;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Web_Api.Repositories.CondidatRepo;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories
{
    public class CondidatRepository : ICondidatRepository
    {
        private readonly QualiProContext _qualiProContext;
        private readonly string _cvFolderPath;
        private readonly string _imageFolderPath;

        public CondidatRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
            _cvFolderPath = "C:\\Users\\LENOVO\\Desktop\\PfERecrutement\\PFERecrutementBack\\QualiPro-Recruitment\\QualiPro-Recruitment-Web-Api\\UploadedCVs";
            _imageFolderPath = "C:\\Users\\LENOVO\\Desktop\\PfERecrutement\\PFERecrutementBack\\QualiPro-Recruitment\\QualiPro-Recruitment-Web-Api\\UploadedImages";
        }

        //public async Task<IEnumerable<CondidatDto>> GetAllCondidatsAsync()
        //{
        //    return await _qualiProContext.TabCondidats.Where(p => p.Deleted == false || p.Deleted == null).ToListAsync();
        //        .Select(c => new CondidatDto
        //        {
        //            Id = c.Id,
        //            Summary = c.Summary,
        //            FirstName = c.FirstName,
        //            LastName = c.LastName,
        //            Country = c.Country,
        //            PhoneNumber = c.PhoneNumber,
        //            Birthdate = c.Birthdate,
        //            ImageFileName = c.ImageFileName,
        //            CvFileName = c.CvFileName
        //        })
        //        .ToListAsync();
        //}
        public async Task<IEnumerable<CondidatDto>> GetAllCondidatsAsync()
        {
            return await _qualiProContext.TabCondidats.OrderByDescending(p => p.Id)
                .Where(p => p.Deleted == false || p.Deleted == null)
                .Select(c => new CondidatDto
                {
                    Id = c.Id,
                    Summary = c.Summary,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Country = c.Country,
                    PhoneNumber = c.PhoneNumber,
                    Birthdate = c.Birthdate,
                    ImageFileName = c.ImageFileName,
                    CvFileName = c.CvFileName
                })
                .ToListAsync(); 
        }

        //public async Task<CondidatDto> GetCondidatByIdAsync(int id)
        //{
        //    var condidat = await _qualiProContext.TabCondidats
        //        .FirstOrDefaultAsync(p => p.Id == id && (p.Deleted == false || p.Deleted == null));
        //    if (condidat == null)
        //    {
        //        return null;
        //    }

        //    return new CondidatDto
        //    {
        //        Id = condidat.Id,
        //        Summary = condidat.Summary,
        //        FirstName = condidat.FirstName,
        //        LastName = condidat.LastName,
        //        Country = condidat.Country,
        //        PhoneNumber = condidat.PhoneNumber,
        //        Birthdate = condidat.Birthdate,
        //        ImageFileName = condidat.ImageFileName,
        //        CvFileName = condidat.CvFileName
        //    };
        //}
        public async Task<CondidatDto> GetCondidatByIdAsync(int id)
        {
            var condidat = await _qualiProContext.TabCondidats
                .FirstOrDefaultAsync(p => p.Id == id && p.Deleted != true);
            if (condidat == null)
            {
                return null;
            }

            return new CondidatDto
            {
                Id = condidat.Id,
                Summary = condidat.Summary,
                FirstName = condidat.FirstName,
                LastName = condidat.LastName,
                Country = condidat.Country,
                PhoneNumber = condidat.PhoneNumber,
                Birthdate = condidat.Birthdate,
                ImageFileName = condidat.ImageFileName,
                CvFileName = condidat.CvFileName
            };
        }


        public async Task<CondidatDto> CreateCondidatAsync(CondidatDto condidatDto)
        {
            var condidat = new TabCondidat()
            {
                Summary = condidatDto.Summary,
                FirstName = condidatDto.FirstName,
                LastName = condidatDto.LastName,
                Country = condidatDto.Country,
                PhoneNumber = condidatDto.PhoneNumber,
                Birthdate = condidatDto.Birthdate
            };

            if (!string.IsNullOrEmpty(condidatDto.ImageFileName))
            {
                condidat.ImageFileName = SaveFile(_imageFolderPath, condidatDto.ImageFileName);
            }

            if (!string.IsNullOrEmpty(condidatDto.CvFileName))
            {
                condidat.CvFileName = SaveFile(_cvFolderPath, condidatDto.CvFileName);
            }

            _qualiProContext.TabCondidats.Add(condidat);
            await _qualiProContext.SaveChangesAsync();

            condidatDto.Id = condidat.Id;
            return condidatDto;
        }

        public async Task UpdateCondidatAsync(CondidatDto condidatDto)
        {
            var condidat = await _qualiProContext.TabCondidats.FindAsync(condidatDto.Id);
            if (condidat == null)
            {
                return;
            }

            condidat.Summary = condidatDto.Summary;
            condidat.FirstName = condidatDto.FirstName;
            condidat.LastName = condidatDto.LastName;
            condidat.Country = condidatDto.Country;
            condidat.PhoneNumber = condidatDto.PhoneNumber;
            condidat.Birthdate = condidatDto.Birthdate;

            if (!string.IsNullOrEmpty(condidatDto.ImageFileName))
            {
                condidat.ImageFileName = SaveFile(_imageFolderPath, condidatDto.ImageFileName);
            }

            if (!string.IsNullOrEmpty(condidatDto.CvFileName))
            {
                condidat.CvFileName = SaveFile(_cvFolderPath, condidatDto.CvFileName);
            }

            _qualiProContext.TabCondidats.Update(condidat);
            await _qualiProContext.SaveChangesAsync();
        }
        public async Task<bool> DeleteCondidatAsync(int id)
        {
            var condidat = await _qualiProContext.TabCondidats.FindAsync(id);
            if (condidat == null)
                return false;
            condidat.Deleted = true;


            await _qualiProContext.SaveChangesAsync();
            return true;
        }

        //public async Task DeleteCondidatAsync(int id)
        //{
        //    var condidat = await _qualiProContext.TabCondidats.FindAsync(id);
        //    if (condidat == null)
        //    {
        //        return;
        //    }

        //    _qualiProContext.TabCondidats.Remove(condidat);
        //    await _qualiProContext.SaveChangesAsync();
        //}

        private string SaveFile(string folderPath, string fileName)
        {
            // Logic to save the file
            var filePath = Path.Combine(folderPath, fileName);
            // Here you can add code to actually move the file to the desired folder
            return fileName;
        }
    }
}
