using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models;
using Rocky.Utility;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private IApplicationTypeRepository _appTypeRepository;

        public ApplicationTypeController(IApplicationTypeRepository appTypeRepository)
        {
            _appTypeRepository = appTypeRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objLists = _appTypeRepository.GetAll();
            return View(objLists);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _appTypeRepository.Add(applicationType);
                _appTypeRepository.Save();
                TempData[WebConstants.Success] = "ApplicationType created successfuly";

                return RedirectToAction("Index");
            }
            TempData[WebConstants.Success] = "Error while created applicationType";
            return View(applicationType);
        }

        //Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id != null || id != 0)
            {
                var appType = _appTypeRepository.Find(id.GetValueOrDefault());
                if (appType != null)
                {
                    return View(appType);
                }
            }

            return NotFound();
        }

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType appType)
        {
            if (ModelState.IsValid)
            {
                _appTypeRepository.Update(appType);
                _appTypeRepository.Save();
                TempData[WebConstants.Success] = "ApplicationType edited successfuly";

                return RedirectToAction("Index");
            }
            return View(appType);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id != null || id != 0)
            {
                var appType = _appTypeRepository.Find(id.GetValueOrDefault());
                if (appType != null)
                {
                    return View(appType);
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
            var appType = _appTypeRepository.Find(id.GetValueOrDefault());
            if (appType == null)
            {
                return NotFound();
            }

            _appTypeRepository.Remove(appType);
            _appTypeRepository.Save();
            TempData[WebConstants.Success] = "ApplicationType deleted successfuly";

            return RedirectToAction("Index");
        }
    }
}
