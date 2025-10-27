namespace ASP.MongoDb.API.Repository
{
  using ASP.MongoDb.API.Entities;
  using ASP.MongoDb.API.DTOs;

  public interface IPropertyRepository : IRepository<Property>
  {
    Task<PropertySearchResultDto> SearchPropertiesAsync(PropertyFilterDto filter);
    Task<Property?> GetPropertyWithDetailsAsync(string id);
    Task<List<Property>> GetPropertiesByOwnerAsync(string ownerId);
    Task<List<Property>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<List<Property>> GetPropertiesByLocationAsync(string city, string? state = null, string? country = null);
    Task<bool> ExistsByCodigoInternoAsync(string codigoInterno);
  }
}