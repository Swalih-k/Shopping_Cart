using Shopping_Cart.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Shopping_Cart.Services
{
    public class CartService
    {
        private readonly List<CartItem> _cartItems = new();
        private int _cartItemId = 1;

        private string _appliedPromoCode;
        private decimal _promoDiscount;

        private bool _isFirstTimeUser = true;

        public List<CartItem> GetCart() => _cartItems;

        public void AddToCart(Product product, int quantity = 1)
        {
            var existingItem = _cartItems.FirstOrDefault(c => c.Product.PId == product.PId);

            if (existingItem != null)
                existingItem.Quantity += quantity;
            else
                _cartItems.Add(new CartItem
                {
                    Id = _cartItemId++,
                    Product = product,
                    Quantity = quantity
                });
        }

        public void RemoveFromCart(int productId)
        {
            var item = _cartItems.FirstOrDefault(c => c.Product.PId == productId);
            if (item != null)
                _cartItems.Remove(item);
        }
        public void ResetPromo()
        {
            _appliedPromoCode = null;
            _promoDiscount = 0;
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(c => c.Product.PId == productId);
            if (item != null)
                item.Quantity = quantity;
        }

        public decimal GetTotal()
        {
            return _cartItems.Sum(item => item.Quantity * item.Product.Price);
        }

        public void ApplyPromo(string code)
        {
            _appliedPromoCode = code?.ToLower();
            _promoDiscount = 0;

            //if (_appliedPromoCode == "save100")
            //{
            //    _promoDiscount = 100;
            //}

            if (_appliedPromoCode == "save100")
            {
                // Apply ₹100 discount per different product
                foreach (var item in _cartItems)
                {

                    _promoDiscount += 100;
                }
            }
           
        
            else if (_appliedPromoCode == "electronics10")
            {
                _promoDiscount = _cartItems
                    .Where(i => i.Product.Category.ToLower() == "electronics")
                    .Sum(i => i.Quantity * i.Product.Price * 0.10m);
            }
        }

        public decimal GetPromoDiscount() => _promoDiscount;

        public string GetAppliedCode() => _appliedPromoCode;

        public decimal GetDiscount()
        {
            decimal discount = 0;
            decimal total = GetTotal();

            // 10% off if total > 50000
            if (total > 50000)
                discount += total * 0.10m;

            // Promo discount
            discount += _promoDiscount;

            // Buy X Get Y Discount
            discount += GetBuyXGetYDiscount();

            // First-time user discount
            if (_isFirstTimeUser && _cartItems.Any())
            {
                var firstItem = _cartItems.First();
                discount += total * 0.05m;
                _isFirstTimeUser = false;
            }

            // Time-based discount 6 PM – 8 PM only
            discount += GetTimeBasedDiscount();

           
            if (discount > total)
                discount = total;

            return discount;
        }

        public decimal GetFinalTotal()
        {
            var total = GetTotal();
            var discount = GetDiscount();
            return Math.Max(0, total - discount);
        }

        public decimal GetBuyXGetYDiscount()
        {
            //decimal discount = 0;
            //var shoesItem = _cartItems.FirstOrDefault(i =>
            //    i.Product.Category.ToLower() == "fashion" &&
            //    i.Product.ProductName.ToLower().Contains("shoes")
            //);

            //if (shoesItem != null && shoesItem.Quantity >= 3)
            //{
            //    int freeItems = shoesItem.Quantity / 3;
            //    discount = freeItems * shoesItem.Product.Price;
            //}
            
            decimal discount = 0;




            foreach (var item in _cartItems) { 
                if (item.Product.Category.ToLower() == "fashion")
                {
                    int freeItems = item.Quantity / 3;

                    // Total discount for this item = freeItems * product price
                    discount += freeItems * item.Product.Price;
                }
                
               
            }

            return discount;
        }

          


        public decimal GetTimeBasedDiscount()
        {
            var now = DateTime.Now.TimeOfDay;
            var start = new TimeSpan(11, 0, 0); // 11:00 
            var end = new TimeSpan(12, 0, 0);   // 06:00 PM

            if (now >= start && now <= end)
                return 100;

            return 0;
        }
    }
}
