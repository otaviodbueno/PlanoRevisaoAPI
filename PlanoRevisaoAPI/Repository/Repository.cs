using PlanoRevisaoAPI.Context;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PlanoRevisaoAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly PlanoRevisaoApiContext _context;

    public Repository(PlanoRevisaoApiContext context)
    {
        _context = context;
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking().ToList();
    }

    public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().AsNoTracking().Where(predicate).ToList();
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        SaveChanges();
        return entity;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        SaveChanges();
    }

    public void DeleteById(int id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            Delete(entity);
        }
    }

    public T Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        SaveChanges();
        return entity;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
