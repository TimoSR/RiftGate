namespace _SharedKernel.Patterns.DomainRules;

public interface IRepository<T> where T : IAggregateRoot
{
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    // You can include more methods as needed, such as Find, GetAll, etc.
}