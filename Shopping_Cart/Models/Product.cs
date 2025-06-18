using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart.Models
{
    public class Product
    {
        [Key]
        public int PId { get; set; }
        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }
    }

}
