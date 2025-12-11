namespace Intec.Workshop1.Customers.Infrastructure;

public interface IUnitOfWork
{
    /// <summary>
    /// Guarda todos los cambios realizados en el contexto de la base de datos
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Número de entidades afectadas</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia una transacción de base de datos
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma la transacción actual
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Revierte la transacción actual
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
