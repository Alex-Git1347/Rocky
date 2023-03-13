using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models;
using Rocky.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDBContext _db;

        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string item)
        {
            IEnumerable<SelectListItem> selectListItem = null;

            if (item == WebConstants.CategoryName)
            {
                selectListItem = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CategoryId.ToString()
                });
            }
            else 
            if (item == WebConstants.ApplicationTypeName)
            {
                selectListItem = _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.AppTypeId.ToString()
                });
            }

            return selectListItem;
        }

        public void Update(Product product)
        {
            _db.Product.Update(product);
        }
    }
}
