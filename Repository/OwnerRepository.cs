namespace ASP.MongoDb.API.Repository
{
  using ASP.MongoDb.API.Entities;
  using Microsoft.Extensions.Options;
  using MongoDB.Driver;
  using MongoDB.Bson;

  public class OwnerRepository : Repository<Owner>, IOwnerRepository
  {
    public OwnerRepository(IOptions<Settings.MongoDbSettings> mongoDbSettings)
        : base(mongoDbSettings)
    {
    }

    public async Task<Owner?> GetByEmailAsync(string email)
    {
      var filter = Builders<Owner>.Filter.Eq(x => x.Email, email);
      return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Owner>> SearchByNameAsync(string name)
    {
      var filter = Builders<Owner>.Filter.Or(
          Builders<Owner>.Filter.Regex(x => x.Name, new BsonRegularExpression(name, "i")),
          Builders<Owner>.Filter.Regex(x => x.LastName, new BsonRegularExpression(name, "i"))
      );

      return await _collection.Find(filter).ToListAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
      var filter = Builders<Owner>.Filter.Eq(x => x.Email, email);
      var count = await _collection.CountDocumentsAsync(filter);
      return count > 0;
    }
  }
}