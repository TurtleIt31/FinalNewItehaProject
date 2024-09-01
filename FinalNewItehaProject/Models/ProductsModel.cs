using FinalProjectIteha.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace ITEHA_Project.Models
{
    public class ProductsModel
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Length cannot be greater than 100")]
        public string ProductName { get; set; } = "New Product";

        [Required]
        public string? ProductDescription { get; set; }

        [Required]
        public int NumberOfProducts { get; set; } = 0;

        [Required]
        public string ProductImage { get; set; } = "Default";

        [Required]
        public int ProductPrice { get; set; } = 1;

        // Collection of ShoppingCartProducts that this product is part of
        public ICollection<ShoppingCartProductModel> ShoppingCartProducts { get; set; } = new List<ShoppingCartProductModel>();
    }

}
