using Microsoft.EntityFrameworkCore;
using NotificationBackendService.Data;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace NotificationBackendService.Repo;

public class GenericRepository : IRepo
{
    protected private AppDataContext _context;
    public GenericRepository(AppDataContext _context) => this._context = _context;

    public async Task<IReadOnlyCollection<T>> ExecuteQueryAsync<T>(string sql, Func<DbDataReader, T> map)
        where T: class, IMustHaveId
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        _context.Database.OpenConnection();

        using var result = await command.ExecuteReaderAsync();
        var entities = new List<T>();

        while (await result.ReadAsync())
        {
            entities.Add(map(result));
        }

        return entities;
    }
}

public abstract class GenericRepository<T> : GenericRepository, IRepo<T>
    where T : class, IMustHaveId
{
    private readonly DbSet<T> entities;

    public GenericRepository(AppDataContext _context) : base(_context)
    {
        entities = _context.Set<T>();
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(string? includesProperty = null)
    {
        var queryEntites = entities.AsQueryable();
        if (!string.IsNullOrEmpty(includesProperty))
        {
            queryEntites = queryEntites.Include(includesProperty);
        }

        return await queryEntites
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> func, string? includesProperty = null)
    {
        var queryEntites = entities.AsQueryable();
        if (!string.IsNullOrEmpty(includesProperty))
        {
            queryEntites = queryEntites.Include(includesProperty);
        }

        return await queryEntites.AsNoTracking()
            .Where(func)
            .ToListAsync();
    }

    public virtual async Task<T?> GetAsync(Guid id, string? includesProperty = null)
    {
        var queryEntites = entities.AsQueryable();
        if (!string.IsNullOrEmpty(includesProperty))
        {
            queryEntites = queryEntites.Include(includesProperty);
        }
        
        return await queryEntites.AsNoTracking()
            .FirstAsync(x => x.Id == id);
    }

    //public T GetById(object id)
    //{
    //    return entities.Find(id);
    //}

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> func, string? includesProperty = null)
    {
        var queryEntites = entities.AsQueryable();
        if (!string.IsNullOrEmpty(includesProperty))
        {
            queryEntites = queryEntites.Include(includesProperty);
        }

        return await queryEntites.AsNoTracking()
            .FirstOrDefaultAsync(func);
    }

    public virtual async Task<T> CreateAsync(T item)
    {
        await entities.AddAsync(item);
        return item;
    }

    public virtual Task UpdateAsync(T item)
    {
        entities.Attach(item);
        _context.Entry(item).State = EntityState.Modified;

        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        T? existing = await GetAsync(id);
        _ = existing ?? throw new ArgumentException(nameof(existing));

        entities.Remove(existing);
    }

    public virtual async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync()
            >= 0;
    }

    public virtual async Task<IReadOnlyCollection<T>> ExecuteQueryAsync(string sql, Func<DbDataReader, T> map)
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        _context.Database.OpenConnection();

        using var result = await command.ExecuteReaderAsync();
        var entities = new List<T>();

        while (await result.ReadAsync())
        {
            entities.Add(map(result));
        }

        return entities;
    }
}
