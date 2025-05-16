using PlanoRevisaoAPI.Context;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using Microsoft.EntityFrameworkCore;

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

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        SaveChanges();
        return entity;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
