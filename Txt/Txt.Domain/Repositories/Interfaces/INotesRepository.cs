using Txt.Domain.Entities;

namespace Txt.Domain.Repositories.Interfaces;

public interface INotesRepository : IRepositoryBase<Note>
{
    /// <summary>
    ///     Guarda en base de datos los cambios hechos
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> observa si la acción fue cancelada.</param>
    /// <returns>
    ///     Un Task con un valor int que representa la cantidad de entidades afectadas
    /// </returns>
    /// <exception cref="DbUpdateException">
    ///     Un error al guardar en base de datos.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    ///     Un error al guardar, al tener un número de entidados diferente al que debería. Generalmente un problema de concurrencia
    /// </exception>
    /// <exception cref="OperationCanceledException">Si <see cref="CancellationToken" /> marca la operación como cancelada.</exception>
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}