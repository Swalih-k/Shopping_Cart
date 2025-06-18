using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product is required.")]
        public Product Product { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
        public decimal Subtotal => Product.Price * Quantity;
       
      

      
    }
}
