

using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ASP.MongoDb.API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;
        public Repository(IOptions<Settings.MongoDbSettings> mongoDbSettings)
        {
            //client of. mongo client
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);

            // database instance
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            // collection from the databases
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        // Retrieves all documents from the collection
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Find all documents and convert the result to a list
            return await _collection.Find(_ => true).ToListAsync();
        }

        // Retrieves a single document by its ID
        public async Task<T> GetByIdAsync(string id)
        {
            // Convert the string 'id' to an ObjectId before querying
            var objectId = new ObjectId(id);

            // Find the document where the '_id' matches the provided ObjectId
            var result = await _collection.Find(Builders<T>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
            return result!;
        }

        // Inserts a new document into the collection
        public async Task CreateAsync(T entity)
        {
            // Add the provided entity to the collection
            await _collection.InsertOneAsync(entity);
        }

        // Updates an existing document by its ID
        public async Task UpdateAsync(string id, T entity)
        {
            // Convert the string 'id' to an ObjectId before querying
            var objectId = new ObjectId(id);

            // Replace the document where the '_id' matches the provided ID with the new entity
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", objectId), entity);
        }

        // Deletes a document by its ID
        public async Task DeleteAsync(string id)
        {
            // Convert the string 'id' to an ObjectId before querying
            var objectId = new ObjectId(id);

            // Remove the document where the '_id' matches the provided ID
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", objectId));
        }

    }
}