using Cassandra;

namespace Gema.Server;

internal class CassandraRepository(ISession session) : IRepository
{
    private readonly ISession _session = session;

  public async Task<IEnumerable<Capsule>> GetAllAsync()
    {
        var query = "SELECT id, body FROM primerIntento.capsules";
        var resultSet = await _session.ExecuteAsync(new SimpleStatement(query));
        return resultSet.Select(row => new Capsule
        {
            Id = row.GetValue<int>("id"),
            Body = row.GetValue<string>("body")
        });
    }

    public async Task<Capsule?> GetByIdAsync(int id)
    {
        var query = "SELECT id, body FROM primerIntento.capsules WHERE id = ?";
        var resultSet = await _session.ExecuteAsync(new SimpleStatement(query, id));
        var row = resultSet.FirstOrDefault();
        return row == null ? null : new Capsule
        {
            Id = row.GetValue<int>("id"),
            Body = row.GetValue<string>("body")
        };
    }

    public async Task AddAsync(Capsule entity)
    {
        var query = "INSERT INTO primerIntento.capsules (id, body) VALUES (?, ?)";
        await _session.ExecuteAsync(new SimpleStatement(query, entity.Id, entity.Body));
    }

    public async Task UpdateAsync(Capsule entity)
    {
        var query = "UPDATE primerIntento.capsules SET body = ? WHERE id = ?";
        await _session.ExecuteAsync(new SimpleStatement(query, entity.Body, entity.Id));
    }

    public async Task DeleteAsync(int id)
    {
        var query = "DELETE FROM primerIntento.capsules WHERE id = ?";
        await _session.ExecuteAsync(new SimpleStatement(query, id));
    }
}
