using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.Image
{
    public interface IImageRepository
    {
        Task<NZWalks.API.Models.Domain.Image> UploadAsync(NZWalks.API.Models.Domain.Image image);
    }
}
