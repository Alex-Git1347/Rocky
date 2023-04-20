﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models;
using Rocky.Models.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IProductRepository _productRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(IWebHostEnvironment webHostEnvironment, IEmailSender emailSender, IProductRepository productRepository, IApplicationUserRepository applicationUserRepository, IInquiryDetailRepository inquiryDetailRepository, IInquiryHeaderRepository inquiryHeaderRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _productRepository = productRepository;
            _applicationUserRepository = applicationUserRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
            _inquiryHeaderRepository = inquiryHeaderRepository;
        }       

        public IActionResult Index()
        {
            var shoppingCartList = GetShoppingCartList();
            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _productRepository.GetAll(u => prodInCart.Contains(u.Id));

            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {   
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCartList = GetShoppingCartList();

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _productRepository.GetAll(u => prodInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _applicationUserRepository.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = productList.ToList()
            };

            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM productUserVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var pathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";
            StringBuilder poductList = new StringBuilder();
            var subject = "New Inquiry";
            string htmlBody = string.Empty;

            using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
            {
                htmlBody = sr.ReadToEnd();
            }

            foreach(var product in productUserVM.ProductList)
            {
                poductList.Append($"  - Name: {product.Name} <span style='font-size:14px;'> (ID: {product.Id})</span><br/>");
            }

            string messageBody = string.Format(htmlBody,
                 ProductUserVM.ApplicationUser.FullName,                  
                 ProductUserVM.ApplicationUser.Email,
                 ProductUserVM.ApplicationUser.PhoneNumber,
                 poductList.ToString());

            await _emailSender.SendEmailAsync(WebConstants.EmailAdmin, subject, messageBody);

            InquiryHeader inquiryHeader = new InquiryHeader()
            {
                AppUserId = claim.Value,
                FullName = ProductUserVM.ApplicationUser.FullName,
                Email = ProductUserVM.ApplicationUser.Email,
                PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                InquiryDate = DateTime.Now
            };

            _inquiryHeaderRepository.Add(inquiryHeader);
            _inquiryHeaderRepository.Save();

            foreach (var product in productUserVM.ProductList)
            {
                InquiryDetail inquiryDetail = new InquiryDetail()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = product.Id,
                };
                _inquiryDetailRepository.Add(inquiryDetail);
            }
            _inquiryDetailRepository.Save();

            return View(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation(ProductUserVM productUserVM)
        {
            HttpContext.Session.Clear();
            return View(ProductUserVM);
        }

        public IActionResult Remove(int id)
        {
            var shoppingCartList = GetShoppingCartList();

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));

            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

            TempData[WebConstants.Success] = "Product deleted successfuly";

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
    }
}
