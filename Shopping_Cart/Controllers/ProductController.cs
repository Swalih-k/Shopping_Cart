using Microsoft.AspNetCore.Mvc;
using Shopping_Cart.Data;
using Shopping_Cart.Models;
using Shopping_Cart.Services;

namespace Shopping_Cart.Controllers
{
    public class ProductController : Controller
    {
        private readonly CartService _cartService;
         private readonly ApplicationDbContext _context;




        public IActionResult ProductUpload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductUpload(Product obj)
        {
            if (ModelState.IsValid)
            {


                _context.product.Add(obj);
                _context.SaveChanges();
                return RedirectToAction(nameof(ProductUpload));
            }
            return View();
        }
        public IActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserLogin(UserLogin Model)
        {
            if (ModelState.IsValid)
            {

                if (Model.Name == "admin" && Model.Password == "123456")
                {

                    return RedirectToAction("Index", "Product");
                }
                else
                {

                    ViewBag.ErrorMessage = "Invalid username or password!";
                }
            }
            return View(Model);
        }











        private static readonly List<Product> Products = new()
        {
            new Product { PId = 1, ProductName = "Computer", Category = "Electronics", Price = 45000 },
            new Product { PId = 2, ProductName = "Laptop", Category = "Electronics", Price = 42000 },
            new Product { PId = 3, ProductName = "Shoes", Category = "Fashion", Price = 2000 },
             new Product { PId = 4, ProductName = "Watch", Category = "Electronics", Price = 1000 },
              new Product { PId = 5, ProductName = "Sunglasses", Category = "Fashion", Price = 2000 }
        };

        public ProductController(ApplicationDbContext context, CartService cartService)
        {
           _context = context;
            _cartService = cartService;
        }

        // Displays available products
        public IActionResult Index()
        {
           var products = _context.product.ToList();
            return View(Products);
        }

        // Adds a product 
        public IActionResult AddToCart(int id)
        {
            var product = Products.FirstOrDefault(p => p.PId == id);
            if (product != null)
            {
                _cartService.AddToCart(product);
            }
            return RedirectToAction("Cart");
        }

        // Displays totals and discounts
        public IActionResult Cart()
        {
            SetCartViewBags();
            return View(_cartService.GetCart());
        }

        // Removes items
        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction("Cart");
        }

        // Updates the quantity 
        [HttpPost]
        public IActionResult UpdateQuantity(int productid, int quantity)
        {
            if (quantity > 0)
            {
                _cartService.UpdateQuantity(productid, quantity);
            }
            return RedirectToAction("Cart");
        }

        // Applies a promo code and updates 
        [HttpPost]
        public IActionResult ApplyPromoCode(string promocode)
        {
            _cartService.ResetPromo();
            _cartService.ApplyPromo(promocode);

            if (_cartService.GetPromoDiscount() > 0)
                ViewBag.PromoMessage = $"Promo code '{promocode}' applied!";
            else
                ViewBag.PromoMessage = $"Promo code '{promocode}' is invalid.";

            SetCartViewBags();
            return View("Cart", _cartService.GetCart());
        }

       
        private void SetCartViewBags()   //view 
        {
            ViewBag.Total = _cartService.GetTotal();
            ViewBag.Discount = _cartService.GetDiscount();
            ViewBag.FinalTotal = _cartService.GetFinalTotal();
        }
        public IActionResult logout()
        {
            return new RedirectResult(url: "/Product/UserLogin", permanent: true,
                preserveMethod: true);
        }
    }
}