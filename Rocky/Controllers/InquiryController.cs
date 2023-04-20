using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.DataAccess.IRepository;
using Rocky.Models;
using Rocky.Models.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    [Authorize(WebConstants.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;

        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailRepository inquiryDetailRepository)
        {
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
        }

        [BindProperty]
        public InquiryVM InquiryVM { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            InquiryVM.InquiryDetails = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == InquiryVM.InquiryHeader.Id);

            foreach (var detailsItem in InquiryVM.InquiryDetails)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detailsItem.ProductId
                };
                shoppingCartList.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);
            HttpContext.Session.Set(WebConstants.SessionInquiryId, InquiryVM.InquiryHeader.Id);

            return RedirectToAction("Index", "Cart");  
        }


        public IActionResult Details(int id)
        {
            InquiryVM = new InquiryVM()
            {
                InquiryHeader = _inquiryHeaderRepository.FirstOrDefault(u => u.Id == id),
                InquiryDetails = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == id, includeProperties: "Product")
            };
            return View(InquiryVM);
        }

        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _inquiryHeaderRepository.FirstOrDefault(u => u.Id == InquiryVM.InquiryHeader.Id);

            _inquiryHeaderRepository.Remove(inquiryHeader);
            _inquiryHeaderRepository.Save();
            TempData[WebConstants.Success] = "Inquiry deleted successfuly";

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _inquiryHeaderRepository.GetAll() });
        }
        #endregion
    }
}
