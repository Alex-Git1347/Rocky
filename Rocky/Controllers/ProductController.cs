using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDBContext _db;

        public ProductController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objLists = _db.Product;
            foreach(var item in objLists)
            {
                item.Category = _db.Category.FirstOrDefault(x => x.CategoryId == item.CategoryId);
            }
            return View(objLists);
        }

        //Get - Upsert
        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            if (id == null)
            {
                //this is for create
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            return View();
        }

        //POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        //Get - Delete
        public IActionResult Delete(int? id)
        {            
            if (id != null || id != 0)
            {
                var category = _db.Category.Find(id);
                if (category != null)
                {
                    return View(category);
                }
            }

            return NotFound();
        }

        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var category = _db.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _db.Category.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");

            return View(category);
        }
    }
}
