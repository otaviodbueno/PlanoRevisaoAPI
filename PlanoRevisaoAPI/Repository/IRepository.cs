using PlanoRevisaoAPI.Models;
using System.Linq.Expressions;

namespace PlanoRevisaoAPI.Repository;

public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    void Delete(T entity);
    void DeleteById(int id);
    void SaveChanges();
}
