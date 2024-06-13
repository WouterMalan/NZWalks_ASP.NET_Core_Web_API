using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.IRepositories;

namespace NZWalks.API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image imageDomainModel)
        {
            var localFilePath = Path.Combine(webHostEnvironment.WebRootPath, "Images", $"{imageDomainModel.FileName}{imageDomainModel.FileExtension}");

            //Upload the file to the local 
            using (var fileStream = new FileStream(localFilePath, FileMode.Create))
            {
                await imageDomainModel.File.CopyToAsync(fileStream);
            }

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{imageDomainModel.FileName}{imageDomainModel.FileExtension}";

            imageDomainModel.FilePath = urlFilePath;

            //Save the image to the database
            await this.dbContext.Images.AddAsync(imageDomainModel);
            await this.dbContext.SaveChangesAsync();

            return imageDomainModel;
        }
    }
}