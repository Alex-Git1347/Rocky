using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections.Generic;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
=======
>>>>>>> 8cdd35f7921731db71aed6bc25da1702bea5abf1

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private ApplicationDBContext _db;

        public ApplicationTypeController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objLists = _db.ApplicationType;
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
<<<<<<< HEAD
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(applicationType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationType);
        }

        //Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id != null || id != 0)
            {
                var appType = _db.ApplicationType.Find(id);
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
                _db.ApplicationType.Update(appType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appType);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id != null || id != 0)
            {
                var appType = _db.ApplicationType.Find(id);
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
            var appType = _db.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }

            _db.ApplicationType.Remove(appType);
            _db.SaveChanges();
            return RedirectToAction("Index");

            //return View(appType);
=======
            _db.ApplicationType.Add(applicationType);
            _db.SaveChanges();
            return RedirectToAction("Index");
>>>>>>> 8cdd35f7921731db71aed6bc25da1702bea5abf1
        }
    }
}
