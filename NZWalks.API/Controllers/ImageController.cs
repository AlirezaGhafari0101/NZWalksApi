using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs.Image;
using NZWalks.API.Repositories.Image;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateUpload(request);
            if (ModelState.IsValid)
            {
                //  dto to domain model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileDescription = request.FileDescription,
                    FileName = request.FileName,
                    FileSizeInBytes = request.File.Length,
                    FileExtension = Path.GetExtension(request.File.FileName)
                };
                await imageRepository.UploadAsync(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }


        private void ValidateUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".png", "jpeg" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size more than 10MB, please upload a smaller file");
            }
        }

    }
}
