using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.DataAccess.Repository;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _productRepository.GetAll(includeProperties :"Category,AppType"),
                Categories = _categoryRepository.GetAll()
            };
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            var shoppingCartList = GetShoppingCartList();

            DetailsVM DetailsVM = new DetailsVM()
            {
                Product = _productRepository.FirstOrDefault(u => u.Id == id, includeProperties: "Category,AppType"),
                ExistsInCart = false
            };

            if (shoppingCartList.FirstOrDefault(p => p.ProductId == id) != null)
            {
                DetailsVM.ExistsInCart = true;
            }

            return View(DetailsVM);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            var shoppingCartList = GetShoppingCartList();
            shoppingCartList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            var shoppingCartList = GetShoppingCartList();
            var shoppingCartItem = shoppingCartList.FirstOrDefault(p => p.ProductId == id);

            if (shoppingCartItem != null)
            {
                shoppingCartList.Remove(shoppingCartItem);
                HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);
            }

            return RedirectToAction(nameof(Index));
        }

        private List<ShoppingCart> GetShoppingCartList()
        {
            var shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            }

            return shoppingCartList;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
