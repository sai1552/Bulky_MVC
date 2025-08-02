using Bulky.DataAccess.Repository.IRepository; //To Access IRepository
using Bulky.DataAcess.Data; //To Access ApplicationDbContext
using Bulky.Models; //To Access Models
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository 
{
    public class ProductRepository : Repository<Product>, IProductRepository //Inherit from ICategoryRepository
    {
        private ApplicationDbContext _db; //To Access Database
        public ProductRepository(ApplicationDbContext db) : base(db) //Inherit from Repository
        {
            _db = db;
        }


        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id); // Get the object from database
            if(objFromDb != null)
            {
                objFromDb.Title = obj.Title; // Update the object
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Author = obj.Author;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100 = obj.Price100;
                
                if(obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
