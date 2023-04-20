using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models;
using Rocky.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Rocky.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objLists = _categoryRepository.GetAll();
            return View(objLists);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Add(category);
                _categoryRepository.Save();
                TempData[WebConstants.Success] = "Category created Successfuly";
                return RedirectToAction("Index");
            }

            TempData[WebConstants.Success] = "Error while creating category";
            return View(category);
        }

        //Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id != null || id != 0)
            {
                var category = _categoryRepository.Find(id.GetValueOrDefault());
                if (category != null)
                {
                    return View(category);
                }
            }

            return NotFound();
        }

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category);
                _categoryRepository.Save();
                TempData[WebConstants.Success] = "Category edited Successfuly";
                return RedirectToAction("Index");
            }
            TempData[WebConstants.Success] = "Error while editing category";
            return View(category);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {            
            if (id != null || id != 0)
            {
                var category = _categoryRepository.Find(id.GetValueOrDefault());
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
            var category = _categoryRepository.Find(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }

            _categoryRepository.Remove(category);
            _categoryRepository.Save();
            TempData[WebConstants.Success] = "Category deleted Successfuly";
            return RedirectToAction("Index");
        }
    }
}
