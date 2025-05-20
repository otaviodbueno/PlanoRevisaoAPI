using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Get(Func<T, bool> predicate);
    T Create(T entity);
    T Update(T entity);
    void Delete(T entity);
    void DeleteById(int id);
    void SaveChanges();
}
