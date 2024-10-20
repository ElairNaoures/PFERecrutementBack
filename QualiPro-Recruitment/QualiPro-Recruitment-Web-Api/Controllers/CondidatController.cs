using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories;
using QualiPro_Recruitment_Web_Api.Repositories.CondidatRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CondidatController : ControllerBase
    {
        private readonly ICondidatRepository _condidatRepository;

        public CondidatController(ICondidatRepository condidatRepository)
        {
            _condidatRepository = condidatRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CondidatDto>> GetAllCondidats()
        {
            return await _condidatRepository.GetAllCondidatsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CondidatDto>> GetCondidatById(int id)
        {
            var condidat = await _condidatRepository.GetCondidatByIdAsync(id);
            if (condidat == null)
            {
                return NotFound();
            }

            return condidat;
        }

        [HttpPost]
        public async Task<ActionResult<CondidatDto>> CreateCondidat([FromForm] CondidatDto condidatDto, IFormFile imageFile, IFormFile cvFile)
        {
            if (imageFile != null)
            {
                var imageFilePath = Path.Combine("UploadedImages", imageFile.FileName);
                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                condidatDto.ImageFileName = imageFile.FileName;
            }

            if (cvFile != null)
            {
                var cvFilePath = Path.Combine("UploadedCVs", cvFile.FileName);
                using (var stream = new FileStream(cvFilePath, FileMode.Create))
                {
                    await cvFile.CopyToAsync(stream);
                }
                condidatDto.CvFileName = cvFile.FileName;
            }

            var createdCondidat = await _condidatRepository.CreateCondidatAsync(condidatDto);
            return CreatedAtAction(nameof(GetCondidatById), new { id = createdCondidat.Id }, createdCondidat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCondidat(int id, [FromForm] CondidatDto condidatDto, IFormFile imageFile, IFormFile cvFile)
        {
            if (id != condidatDto.Id)
            {
                return BadRequest();
            }

            if (imageFile != null)
            {
                var imageFilePath = Path.Combine("UploadedImages", imageFile.FileName);
                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                condidatDto.ImageFileName = imageFile.FileName;
            }

            if (cvFile != null)
            {
                var cvFilePath = Path.Combine("UploadedCVs", cvFile.FileName);
                using (var stream = new FileStream(cvFilePath, FileMode.Create))
                {
                    await cvFile.CopyToAsync(stream);
                }
                condidatDto.CvFileName = cvFile.FileName;
            }

            await _condidatRepository.UpdateCondidatAsync(condidatDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCondidat(int id)
        {
            await _condidatRepository.DeleteCondidatAsync(id);
            return NoContent();
        }
        //[HttpGet("DownloadCV/{fileName}")]
        //public IActionResult DownloadCV(string fileName)
        //{
        //    // Le chemin absolu où les fichiers sont stockés
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedCVs", fileName);

        //    // Vérifiez si le fichier existe
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return NotFound(new { message = "File not found." });
        //    }

        //    // Renvoyez le fichier avec le type MIME correct
        //    var mimeType = "application/pdf";
        //    return PhysicalFile(filePath, mimeType, fileName);
        //}

        [HttpGet("DownloadCV/{fileName}")]
        public IActionResult DownloadCV(string fileName)
        {
            var filePath = Path.Combine("UploadedCVs", fileName); // Assurez-vous que ce chemin est correct

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Si le fichier n'existe pas
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", fileName);
        }


    }
}
