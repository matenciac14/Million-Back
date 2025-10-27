namespace ASP.MongoDb.API.Repository
{
  using ASP.MongoDb.API.Entities;

  public interface IPropertyTraceRepository : IRepository<PropertyTrace>
  {
    Task<List<PropertyTrace>> GetByPropertyIdAsync(string propertyId);
    Task DeleteByPropertyIdAsync(string propertyId);
  }
}