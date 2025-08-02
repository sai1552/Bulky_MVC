using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Repository.IRepository; //Added namespace for finding IRepository.
using Bulky.DataAcess.Data;
using Microsoft.EntityFrameworkCore; 

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class  // make sure to implement IRepository<T> interface.
    {
        private readonly ApplicationDbContext _db; //Added ApplicationDbContext for database connection.And it is dependency injection.
        internal DbSet<T> dbSet; //Added DbSet for database connection.

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            //_db.Categories == dbset
            //dbSet.Add();
            _db.Products.Include(u=> u.Category).Include(u=>u.CategoryId); //This is used to include the category in the product.
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet; //We are assignied complete dbSet to query.
            query = query.Where(filter); //We are filtering the query.
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault(); //We are returning the first element of the query.
        }

        //Category, CategoryId must match with the include model.
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
