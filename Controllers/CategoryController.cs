using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using CRUD.Data;
using CRUD;
using Microsoft.AspNetCore.Authorization;

namespace Info.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _DB;
        public CategoryController(ApplicationDBContext DB)
        {
            _DB = DB;
        }
        public IActionResult Index()
        {

            IEnumerable<Category> objcategoriesList = _DB.Categories;
            return View(objcategoriesList);
        }
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _DB.Categories.Add(obj);
                _DB.SaveChanges();
                TempData["success"] = "Data Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _DB.Categories.Find(Id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _DB.Categories.Update(obj);
                _DB.SaveChanges();
                TempData["success"] = "Data Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _DB.Categories.Find(Id);
            /*var categoryFromDbFirst = _DB.categories.FirstOrDefault(u => u.Id == Id);
            var categoryFromDbsingle = _DB.categories.SingleOrDefault(u => u.Id == Id);*/
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            var obj = _DB.Categories.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }

            _DB.Categories.Remove(obj);
            _DB.SaveChanges();
            TempData["success"] = "Data Deleted Successfully";
            return RedirectToAction("Index");



        }
    }
}





