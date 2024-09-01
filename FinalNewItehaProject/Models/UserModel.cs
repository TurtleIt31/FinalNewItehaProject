using ITEHA_Project.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalNewItehaProject.Models
{
    public class UserModel 
    {
        [Key]
        public int UserId { get; set; }
        // Additional properties for your UserModel

        
        public string UserName { get; set; } = string.Empty;

        [Required] public string Password { get; set; }= string.Empty;

        [Required] public string Email { get; set; } = string.Empty;

        [Required]
        public string UserType { get; set; } = "Customer";

        // Collection of Shopping Carts owned by the User
        public ICollection<ShoppingCartModel> ShoppingCarts { get; set; } = new List<ShoppingCartModel>();
    }
}
