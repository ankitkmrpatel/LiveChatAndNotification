using System.Data.Common;
using System.Linq.Expressions;

namespace NotificationBackendService.Data;

public interface IRepo
{
    Task<IReadOnlyCollection<T>> ExecuteQueryAsync<T>(string sql, Func<DbDataReader, T> map)
        where T : class, IMustHaveId;
}

public interface IRepo<T>
    where T : class, IMustHaveId
{
    Task<IReadOnlyCollection<T>> GetAllAsync(string? includesProperty = null);
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> func, string? includesProperty = null);
    Task<T?> GetAsync(Guid id, string? includesProperty = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> func, string? includesProperty = null);
    Task<T> CreateAsync(T item);
    Task UpdateAsync(T item);
    Task DeleteAsync(Guid id);
    Task<bool> SaveChangesAsync();
}
