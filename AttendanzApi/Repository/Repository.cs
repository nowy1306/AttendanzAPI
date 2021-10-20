using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Interfaces;
using AttendanzApi.Db;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AttendanzApi.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IModel
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] properties)
        {
            var query = _context.Set<T>().AsQueryable();
            query = properties.Aggregate(query, (current, property) => current.Include(property));

            return query.AsEnumerable();
        }

        public T GetById(long id)
        {
            return _context.Set<T>().FirstOrDefault(model => id == model.Id);
        }

        public T GetById(long id, params Expression<Func<T, object>>[] properties)
        {
            var query = _context.Set<T>().AsQueryable();
            query = properties.Aggregate(query, (current, property) => current.Include(property));

            return query.FirstOrDefault(model => id == model.Id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] properties)
        {
            var query = _context.Set<T>().AsQueryable();
            query = properties.Aggregate(query, (current, property) => current.Include(property));

            return query.FirstOrDefault(predicate);
        }

        public long Insert(T model)
        {
            _context.Set<T>().Add(model);
            _context.SaveChanges();
            return model.Id;
        }

        public void Update(T model)
        {
            _context.SaveChanges();
        }
    }
}
