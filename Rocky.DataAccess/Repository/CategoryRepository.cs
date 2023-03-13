using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDBContext _db;

        public CategoryRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var objFromDb = base.FirstOrDefault(u => u.CategoryId == category.CategoryId);
            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
                objFromDb.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
