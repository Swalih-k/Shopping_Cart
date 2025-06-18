using Shopping_Cart.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shopping_Cart.Services
{
    public class CartService
    {
        private readonly List<CartItem> _cartItems = new();
        private int _cartItemId = 1;

        private string _appliedPromoCode;
        private decimal _promoDiscount;

        private bool _isFirstTimeUser = true;

        // Return the current list of cart items
        public List<CartItem> GetCart() => _cartItems;

        // Add a product to the cart or increase its quantity if it already exists
        public void AddToCart(Product product, int quantity = 1)
        {
            var existingItem = _cartItems.FirstOrDefault(c => c.Product.PId == product.PId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    Id = _cartItemId++,
                    Product = product,
                    Quantity = quantity
                });
            }
        }

        // Remove an item from the cart based on ProductId
        public void RemoveFromCart(int productId)
        {
            var item = _cartItems.FirstOrDefault(c => c.Product.PId == productId);
            if (item != null)
            {
                _cartItems.Remove(item);
            }
        }

        // Update quantity of a product in the cart
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(c => c.Product.PId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }

        // Calculate the total cost of the cart before any discounts
        public decimal GetTotal() => _cartItems.Sum(item => item.Subtotal);

        // Apply a promotional discount code
        public void ApplyPromo(string code)
        {
            _appliedPromoCode = code?.ToLower();
            _promoDiscount = 0;

            if (_appliedPromoCode == "save100")
            {
                _promoDiscount = 100;
            }
            else if (_appliedPromoCode == "electronics20")
            {
                _promoDiscount = _cartItems
                    .Where(i => i.Product.Category.ToLower() == "electronics")
                    .Sum(i => i.Subtotal * 0.20m);
            }
        }

        public decimal GetPromoDiscount() => _promoDiscount;

        public string GetAppliedCode() => _appliedPromoCode;

        // Get total discount (promo, 10% if total > 50000, buy X get Y, first-time)
        public decimal GetDiscount()
        {
            decimal discount = 0;
            var total = GetTotal();

            if (GetTotal() > 50000)
                discount += GetTotal() * 0.10m;

            discount += _promoDiscount;
            discount += GetBuyXGetYDiscount();
            discount += GetFirstTimeUserDiscount();
            discount += GetTimeBasedDiscount(); 

            return discount;
        }

      

        // Calculate final total after discounts
        public decimal GetFinalTotal() => GetTotal() - GetDiscount();

        // Buy 2 Get 1 Free discount for Shoes in Fashion category
        public decimal GetBuyXGetYDiscount()
        {
            decimal discount = 0;
            var shoesItem = _cartItems.FirstOrDefault(i =>
                i.Product.Category.ToLower() == "fashion" &&
                i.Product.ProductName.ToLower().Contains("shoes")
            );

            if (shoesItem != null && shoesItem.Quantity >= 3)
            {
                int freeItems = shoesItem.Quantity / 3;
                discount = freeItems * shoesItem.Product.Price;
            }
            return discount;
        }

        // 5% first-time user discount
        public decimal GetFirstTimeUserDiscount()
        {
            if (_isFirstTimeUser)
            {
                return GetTotal() * 0.05m;
            }
            return 0;
        }
        public decimal GetTimeBasedDiscount()
        {
            var now = DateTime.Now.TimeOfDay;

            
            var start = new TimeSpan(18, 0, 0); // 6:00 PM
            var end = new TimeSpan(20, 0, 0);   // 8:00 PM

            if (now >= start && now <= end)
            {
                return 100; 
            }

            return 0;
        }

    }
}