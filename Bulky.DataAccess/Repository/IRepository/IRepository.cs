using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category
        IEnumerable<T> GetAll(string? includeProperties = null); //Return type is IEnumerable
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null); //Link Expression
        void Add(T entity);
        //void Update(T entity); //We are commenting this because we are don't want to save changes in the database.
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity); //collection of entities

    }
}
