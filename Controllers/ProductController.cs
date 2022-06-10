using CRUD_Models;

using CRUD_Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD.Data;
using static System.Net.Mime.MediaTypeNames;
using CRUD;
using Microsoft.AspNetCore.Authorization;
using CRUD_Utility;

namespace Info.Controllers
{

    [Authorize(Roles=WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;

        }



        //get
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _db.Product;
            foreach (var obj in objProductList)
            {
                obj.Category = _db.Categories.FirstOrDefault(u => u.Id == obj.CategoryId);
                obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.id == obj.Applicationid);

            }
            return View(objProductList);

        }
        //Get Upsert
        public IActionResult Upsert(int? Id)
        {
            IEnumerable<SelectListItem> CategoryDropdown = _db.Categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            ViewBag.CategoryDropdown = CategoryDropdown;

            IEnumerable<SelectListItem> ApplicationDropdown = _db.ApplicationType.Select(i => new SelectListItem
            {
                Text = i.name,
                Value = i.id.ToString()

            });
            ViewBag.ApplicationDropdown = ApplicationDropdown;
            Product product = new Product();
            if (Id == null)
            {
                return View(product);
            }
            else
            {
                product = _db.Product.Find(Id);

                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            return View();


        }



        //Post Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                String webRootpath = _webHostEnvironment.WebRootPath;
                if (obj.Id == 0)
                {
                    string upload = webRootpath + WC.Imagepath;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var filestream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    obj.Image = filename + extension;
                    _db.Product.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "Data Created Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    var objFromdb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == obj.Id);
                    if (files.Count() > 0)
                    {
                        string upload = webRootpath + WC.Imagepath;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        var oldfile = Path.Combine(upload, obj.Image);
                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }
                        using (var filestream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                        obj.Image = filename + extension;
                    }
                    else
                    {
                        obj.Image = objFromdb.Image;
                    }
                    _db.Product.Update(obj);
                }


                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            IEnumerable<SelectListItem> CategoryDropdown = _db.Categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            ViewBag.CategoryDropdown = CategoryDropdown;

            IEnumerable<SelectListItem> ApplicationDropdown = _db.ApplicationType.Select(i => new SelectListItem
            {
                Text = i.name,
                Value = i.id.ToString()

            });
            ViewBag.ApplicationDropdown = ApplicationDropdown;
            return View();
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType).FirstOrDefault(u => u.Id == Id);
            // product.Category = _db.Category.Find(product.Category.Id);


            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WC.Imagepath;
            var oldfile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }
            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");



        }




    }
}