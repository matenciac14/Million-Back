namespace ASP.MongoDb.API.Repository
{
  using ASP.MongoDb.API.Entities;

  public interface IOwnerRepository : IRepository<Owner>
  {
    Task<Owner?> GetByEmailAsync(string email);
    Task<List<Owner>> SearchByNameAsync(string name);
    Task<bool> ExistsByEmailAsync(string email);
  }
}