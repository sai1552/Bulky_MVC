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
    public class CategoryRepository : Repository<Category>, ICategoryRepository //Inherit from ICategoryRepository
    {
        private ApplicationDbContext _db; //To Access Database
        public CategoryRepository(ApplicationDbContext db) : base(db) //Inherit from Repository
        {
            _db = db;
        }


        public void Update(Category obj)
        {
            _db.Categories.Update(obj); //To Update
        }
    }
}
