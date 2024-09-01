using ITEHA_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectIteha.Models
{
    public class ShoppingCartProductModel
    {
        [Key]
        public int ShoppingCartProductId { get; set; }

        // Foreign key to ShoppingCartModel
        public int ShoppingCartId { get; set; }
        public ShoppingCartModel ShoppingCart { get; set; } = new ShoppingCartModel();

        // Foreign key to ProductsModel
        public int ProductId { get; set; }
        public ProductsModel Product { get; set; } = new ProductsModel();

        [Required]
        public int Quantity { get; set; } = 1; // Default quantity is 1
    }
}
