using ASP.MongoDb.API.Entities;

namespace ASP.MongoDb.API.Repository
{
  public interface IPropertyImageRepository : IRepository<PropertyImage>
  {
    Task<List<PropertyImage>> GetByPropertyIdAsync(string propertyId);
    Task<PropertyImage?> GetMainImageByPropertyIdAsync(string propertyId);
    Task<bool> SetMainImageAsync(string propertyId, string imageId);
    Task<bool> DeleteByPropertyIdAsync(string propertyId);
    Task<bool> DeleteByCloudinaryPublicIdAsync(string publicId);
    Task<List<PropertyImage>> GetEnabledByPropertyIdAsync(string propertyId);
  }
}