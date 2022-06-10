using CRUD_Models;
using CRUD_Models.ViewModel;
using CRUD_Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CRUD.Data;
using CRUD;

namespace Info.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()


        {
            HomeVM homeVM = new HomeVM();
            {
                homeVM.Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType);
                homeVM.Categories = _db.Categories;



            }


            return View(homeVM);

        }
        public IActionResult Details(int Id)
        {
            List<ShoppingCart> shoppingCartlist = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartlist = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            //Product product = new Product();
            DetailsVM detailsVM = new DetailsVM();
            {

                detailsVM.Product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType).Where(u => u.Id == Id).FirstOrDefault();
                detailsVM.ExitsInCart = false;
            };
            foreach (var item in shoppingCartlist)
            {
                if (item.ProductId == Id)
                {
                    detailsVM.ExitsInCart = true;
                }
            }
            return View(detailsVM);
        }
        [HttpPost, ActionName("Details")]

        public IActionResult DetailsPost(int Id)
        {
            List<ShoppingCart> shoppingCartlist = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartlist = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartlist.Add(new ShoppingCart { ProductId = Id });
            HttpContext.Session.Set(WC.SessionCart, shoppingCartlist);


            return RedirectToAction(nameof(Index));

        }
        public IActionResult RemovefromCart(int Id)
        {
            List<ShoppingCart> shoppingCartlist = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartlist = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            var itemToRemove = shoppingCartlist.SingleOrDefault(r => r.ProductId == Id);
            if (itemToRemove != null)
            {
                shoppingCartlist.Remove(itemToRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCartlist);


            return RedirectToAction(nameof(Index));

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
