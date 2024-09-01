using FinalNewItehaProject.Models;
using FinalProjectIteha.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITEHA_Project.Models
{
    public class ShoppingCartModel
    {
        [Key]
        [Required]
        public int ShoppingCartId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        // Collection of ShoppingCartProducts (products in this cart)
        public ICollection<ShoppingCartProductModel> ShoppingCartProducts { get; set; } = new List<ShoppingCartProductModel>();

        // Computed property to calculate the total price of all products in the cart
        [NotMapped]
        public int TotalPrice
        {
            get
            {
                return ShoppingCartProducts?.Sum(p => p.Product.ProductPrice * p.Quantity) ?? 0;
            }
        }

        // Computed property to calculate the total number of items in the cart
        [NotMapped]
        public int TotalItems
        {
            get
            {
                return ShoppingCartProducts?.Sum(p => p.Quantity) ?? 0;
            }
        }
    }
}
