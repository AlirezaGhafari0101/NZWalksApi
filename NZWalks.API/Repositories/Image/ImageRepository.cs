using NZWalks.API.Data;

namespace NZWalks.API.Repositories.Image
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext ctx;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor
            , NZWalksDbContext ctx)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.ctx = ctx;
        }
        public async Task<Models.Domain.Image> UploadAsync(Models.Domain.Image image)
        {
            var localeFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
               $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localeFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            await ctx.Images.AddAsync(image);
            await ctx.SaveChangesAsync();

            return image;
        }
    }
}
