using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttendanzApi.Interfaces
{
    public interface IRepository<T> where T : IModel
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] properties);
        T GetById(long id);
        T GetById(long id, params Expression<Func<T, object>>[] properties);
        T FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] properties);
        long Insert(T model);
        void Update(T model);
        void Delete(T model);
    }
}
