namespace ASP.MongoDb.API.Repository
{
  using ASP.MongoDb.API.Entities;
  using ASP.MongoDb.API.Settings;
  using MongoDB.Driver;
  using Microsoft.Extensions.Options;

  public class PropertyTraceRepository : Repository<PropertyTrace>, IPropertyTraceRepository
  {
    public PropertyTraceRepository(IOptions<MongoDbSettings> settings) : base(settings)
    {
    }

    public async Task<List<PropertyTrace>> GetByPropertyIdAsync(string propertyId)
    {
      var filter = Builders<PropertyTrace>.Filter.Eq(x => x.IdProperty, propertyId);
      var traces = await _collection.Find(filter)
                                   .SortByDescending(x => x.DateSale)
                                   .ToListAsync();
      return traces;
    }

    public async Task DeleteByPropertyIdAsync(string propertyId)
    {
      var filter = Builders<PropertyTrace>.Filter.Eq(x => x.IdProperty, propertyId);
      await _collection.DeleteManyAsync(filter);
    }
  }
}