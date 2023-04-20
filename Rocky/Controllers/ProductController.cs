using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rocky.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objLists = _productRepository.GetAll(includeProperties: "Category,AppType");
            return View(objLists);
        }

        //Get - Upsert
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _productRepository.GetAllDropdownList(WebConstants.CategoryName),
                AppTypeSelectList = _productRepository.GetAllDropdownList(WebConstants.ApplicationTypeName)
            };

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _productRepository.Find(id.GetValueOrDefault());
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        //POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    var upload = webRootPath + WebConstants.ImagePath;
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;
                    _productRepository.Add(productVM.Product);
                    TempData[WebConstants.Success] = "Product created successfuly";
                }
                else
                {
                    var product = _productRepository.FirstOrDefault(x => x.Id == productVM.Product.Id, isTracking:false);

                    if (files.Count > 0)
                    {
                        var upload = webRootPath + WebConstants.ImagePath;
                        var fileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, product.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = product.Image;
                    }

                    _productRepository.Update(productVM.Product);
                    TempData[WebConstants.Success] = "Product updated successfuly";
                }

                _productRepository.Save();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _productRepository.GetAllDropdownList(WebConstants.CategoryName);
            productVM.AppTypeSelectList = _productRepository.GetAllDropdownList(WebConstants.ApplicationTypeName);
            TempData[WebConstants.Success] = string.Format("Error while is not valid category");

            return View(productVM);
        }


        //Get - Delete
        public IActionResult Delete(int? id)
        {            
            if (id != null || id != 0)
            {
                var product = _productRepository.FirstOrDefault(u => u.Id == id, includeProperties:"Category,AppType");
                if (product != null)
                {
                    return View(product);
                }
            }

            return NotFound();
        }

        //Post - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _productRepository.Find(id.GetValueOrDefault());
            if (product == null)
            {
                return NotFound();
            }

            var webRootPath = _webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(product.Image))                
            {                    
                var upload = webRootPath + WebConstants.ImagePath;                                    
                var imageFilePath = Path.Combine(upload, product.Image);
                    
                if (System.IO.File.Exists(imageFilePath))                    
                {                        
                    System.IO.File.Delete(imageFilePath);                    
                }                
            }

            _productRepository.Remove(product);
            _productRepository.Save();
            TempData[WebConstants.Success] = "Product deleted successfuly";

            return RedirectToAction("Index");
        }
    }
}
