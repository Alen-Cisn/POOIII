internal interface IRepository
{
  public Task<IEnumerable<Capsule>> GetAllAsync();
  public Task<Capsule?> GetByIdAsync(int id);
  public Task AddAsync(Capsule entity);
  public Task UpdateAsync(Capsule entity);
  public Task DeleteAsync(int id);
}
