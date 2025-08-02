using System.Linq.Expressions;

namespace BARQ.Core.Repositories;

/// <summary>
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, 
                                  Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                  int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    IQueryable<T> GetQueryable();
}

/// <summary>
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
