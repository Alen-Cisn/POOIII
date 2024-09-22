using Microsoft.Data.SqlClient;

namespace Gema.Server;

internal class SqlServerRepository(SqlConnection connection) : IRepository
{
  private readonly SqlConnection _connection = connection;

  public async Task<IEnumerable<Capsule>> GetAllAsync()
  {
    throw new NotImplementedException();
  }

  public async Task<Capsule?> GetByIdAsync(int id)
  {
     throw new NotImplementedException();
  }

  public async Task AddAsync(Capsule entity)
  {
     throw new NotImplementedException();
  }

  public async Task UpdateAsync(Capsule entity)
  {
     throw new NotImplementedException();
  }

  public async Task DeleteAsync(int id)
  {
     throw new NotImplementedException();
  }
}
