using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories.IRepositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // Post: api/images/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadDto)
        {
            ValidateFileUpload(imageUploadDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //convert DTO to domain model
            var imageDomainModel = new Image
            {
                File = imageUploadDto.File,
                FileExtension = Path.GetExtension(imageUploadDto.File.FileName),
                FileSizeInBytes = imageUploadDto.File.Length,
                FileName = imageUploadDto.FileName,
                FileDescription = imageUploadDto.FileDescription
            };

            // User repository to upload the image
            await imageRepository.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }


        #region Private methods

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadDto)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            if (imageUploadDto.File == null)
            {
                throw new ArgumentNullException("File", "File is required");
            }

            if (!allowedExtensions.Contains(Path.GetExtension(imageUploadDto.File.FileName).ToLower()))
            {
                ModelState.AddModelError("File", "Invalid file type. Only .jpg, .jpeg, .png files are allowed");
            }

            // File size must be less than 10MB
            if (imageUploadDto.File.Length > 10485760)
            {
                ModelState.AddModelError("File", "File size must be less than 10MB");
            }


        }

        #endregion
    }
}