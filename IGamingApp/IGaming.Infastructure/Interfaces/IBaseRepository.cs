namespace IGaming.Infrastructure.Interfaces;
public interface IBaseRepository<T> where T : class
{
    IQueryable<T> Table { get; }
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    void Delete(T entity);
}
