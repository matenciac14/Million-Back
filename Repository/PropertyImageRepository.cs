using ASP.MongoDb.API.Entities;
using ASP.MongoDb.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ASP.MongoDb.API.Repository
{
  public class PropertyImageRepository : Repository<PropertyImage>, IPropertyImageRepository
  {
    public PropertyImageRepository(IOptions<MongoDbSettings> mongoDbSettings)
      : base(mongoDbSettings)
    {
    }

    public async Task<List<PropertyImage>> GetByPropertyIdAsync(string propertyId)
    {
      var filter = Builders<PropertyImage>.Filter.Eq(x => x.IdProperty, propertyId);
      return await _collection.Find(filter).ToListAsync();
    }

    public async Task<PropertyImage?> GetMainImageByPropertyIdAsync(string propertyId)
    {
      var filter = Builders<PropertyImage>.Filter.And(
        Builders<PropertyImage>.Filter.Eq(x => x.IdProperty, propertyId),
        Builders<PropertyImage>.Filter.Eq(x => x.IsMain, true),
        Builders<PropertyImage>.Filter.Eq(x => x.Enabled, true)
      );

      return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> SetMainImageAsync(string propertyId, string imageId)
    {
      // First, remove IsMain flag from all images of this property
      var filterAll = Builders<PropertyImage>.Filter.Eq(x => x.IdProperty, propertyId);
      var updateAll = Builders<PropertyImage>.Update.Set(x => x.IsMain, false);
      await _collection.UpdateManyAsync(filterAll, updateAll);

      // Then set the specific image as main
      var filterMain = Builders<PropertyImage>.Filter.And(
        Builders<PropertyImage>.Filter.Eq(x => x.IdProperty, propertyId),
        Builders<PropertyImage>.Filter.Eq(x => x.Id, imageId)
      );
      var updateMain = Builders<PropertyImage>.Update.Set(x => x.IsMain, true);
      var result = await _collection.UpdateOneAsync(filterMain, updateMain);

      return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteByPropertyIdAsync(string propertyId)
    {
      var filter = Builders<PropertyImage>.Filter.Eq(x => x.IdProperty, propertyId);
      var result = await _collection.DeleteManyAsync(filter);
      return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteByCloudinaryPublicIdAsync(string publicId)
    {
      var filter = Builders<PropertyImage>.Filter.Eq(x => x.CloudinaryPublicId, publicId);
      var result = await _collection.DeleteOneAsync(filter);
      return result.DeletedCount > 0;
    }

    public async Task<List<PropertyImage>> GetEnabledByPropertyIdAsync(string propertyId)
    {
      var filter = Builders<PropertyImage>.Filter.And(
        Builders<PropertyImage>.Filter.Eq(x => x.IdProperty, propertyId),
        Builders<PropertyImage>.Filter.Eq(x => x.Enabled, true)
      );

      var sort = Builders<PropertyImage>.Sort
        .Descending(x => x.IsMain)
        .Ascending(x => x.CreatedAt);

      return await _collection.Find(filter).Sort(sort).ToListAsync();
    }
  }
}