using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using CRUD.Data;

namespace Info.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDBContext _DB;
        public ApplicationTypeController(ApplicationDBContext DB)
        {
            _DB = DB;
        }
        public IActionResult Index()
        {

            IEnumerable<ApplicationType> objApplicationTypeList = _DB.ApplicationType;
            return View(objApplicationTypeList);
        }
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _DB.ApplicationType.Add(obj);
                _DB.SaveChanges();
                TempData["success"] = "Data Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //get Edit
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var ApplicationTypeFromDb = _DB.ApplicationType.Find(Id);

            if (ApplicationTypeFromDb == null)
            {
                return NotFound();
            }
            return View(ApplicationTypeFromDb);
        }
        //post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {

            if (ModelState.IsValid)
            {
                _DB.ApplicationType.Update(obj);
                _DB.SaveChanges();
                TempData["success"] = "Data Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //get Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ApplicationTypeFromDb = _DB.ApplicationType.Find(id);
            /*var categoryFromDbFirst = _DB.categories.FirstOrDefault(u => u.Id == Id);
            var categoryFromDbsingle = _DB.categories.SingleOrDefault(u => u.Id == Id);*/
            if (ApplicationTypeFromDb == null)
            {
                return NotFound();
            }
            return View(ApplicationTypeFromDb);
        }
        //post Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _DB.ApplicationType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _DB.ApplicationType.Remove(obj);
            _DB.SaveChanges();
            TempData["success"] = "Data Deleted Successfully";
            return RedirectToAction("Index");

        }
    }
}
