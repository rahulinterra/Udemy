using CRUD.Data;
using CRUD.Models;
using CRUD.Models.ViewModel;
using CRUD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace CRUD.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        public ProductUserVm ProductUserVm { get; set; }
        public CartController (ApplicationDBContext db, IWebHostEnvironment webHostEnvironment,IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = _db.Product.Where(u => prodInCart.Contains(u.Id));
            return View(prodList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName ("Index")]
        public IActionResult IndexPost()
        {
                      return RedirectToAction(nameof(Summary));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var UserId = User.FindFirst(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = _db.Product.Where(u => prodInCart.Contains(u.Id));

            ProductUserVm  = new ProductUserVm()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claims.Value),
                ProductList = prodList.ToList(),
            };
            return View(ProductUserVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVm ProductUserVm)
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
            + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";
            var subject = "New Inquiry";
            string Htmlbody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                Htmlbody = sr.ReadToEnd();
            }
            StringBuilder productListSB = new StringBuilder();
            foreach(var product in ProductUserVm.ProductList)
            {
                productListSB.Append($"- Name :{product.Name}<span style='font-size:14px';>(ID:{product.Id})</span></br>");

            }
            string Messagebody = string.Format(Htmlbody, ProductUserVm.ApplicationUser.FullName
                , ProductUserVm.ApplicationUser.Email,
                ProductUserVm.ApplicationUser.PhoneNumber,
                productListSB.ToString());
            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, Messagebody);

            return RedirectToAction(nameof(InquiryConfirmation));
        }
        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction("Index");
        }

    }
}
