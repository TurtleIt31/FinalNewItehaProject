using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinalNewItehaProject.Models;
using ITEHA_Project.Models;
using FinalProjectIteha.Models;


namespace FinalNewItehaProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FinalNewItehaProject.Models.UserModel>? UserModel { get; set; }
        public DbSet<ITEHA_Project.Models.ShoppingCartModel>? ShoppingCartModel { get; set; }
        public DbSet<FinalProjectIteha.Models.ShoppingCartProductModel>? ShoppingCartProductModel { get; set; }
        public DbSet<ITEHA_Project.Models.ProductsModel>? ProductsModel { get; set; }
    }
}