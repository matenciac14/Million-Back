namespace ASP.MongoDb.API.ProductRepository
{
  using ASP.MongoDb.API.Entities;
  using ASP.MongoDb.API.Repository;
  using Microsoft.Extensions.Options;

  public interface IProductRepository : IRepository<Products>
  {
  }
  public class ProductRepository : Repository<Products>, IProductRepository
  {
    public ProductRepository(IOptions<Settings.MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
    {
    }
  }
}