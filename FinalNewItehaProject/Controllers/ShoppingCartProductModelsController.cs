using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalNewItehaProject.Data;
using FinalProjectIteha.Models;
using ITEHA_Project.Models;

namespace FinalNewItehaProject.Controllers
{
    public class ShoppingCartProductModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        //Post for add to cart method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            // Ensure the user is logged in
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "UserModels");
            }

            // Get the user's shopping cart
            var shoppingCart = await _context.ShoppingCartModel
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // If the user does not have a shopping cart, create one
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCartModel
                {
                    UserId = userId.Value
                };
                _context.ShoppingCartModel.Add(shoppingCart);
                await _context.SaveChangesAsync();
            }

            // Ensure the product exists
            var product = await _context.ProductsModel.FindAsync(productId);
            if (product == null)
            {
                // Handle the case where the product does not exist
                return NotFound();
            }

            // Check if the product is already in the cart
            var cartItem = await _context.ShoppingCartProductModel
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.ShoppingCartId == shoppingCart.ShoppingCartId);

            if (cartItem != null)
            {
                // If the item is already in the cart, increase the quantity
                cartItem.Quantity++;
                _context.Update(cartItem);
            }
            else
            {
                // If the item is not in the cart, add it
                cartItem = new ShoppingCartProductModel
                {
                    ShoppingCartId = shoppingCart.ShoppingCartId,
                    ProductId = productId,
                    Quantity = 1
                };
                _context.ShoppingCartProductModel.Add(cartItem);
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect back to the Products page to allow further browsing
            return RedirectToAction("Index", "Products");
        }

        public ShoppingCartProductModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCartProductModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShoppingCartProductModel.Include(s => s.Product).Include(s => s.ShoppingCart);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShoppingCartProductModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShoppingCartProductModel == null)
            {
                return NotFound();
            }

            var shoppingCartProductModel = await _context.ShoppingCartProductModel
                .Include(s => s.Product)
                .Include(s => s.ShoppingCart)
                .FirstOrDefaultAsync(m => m.ShoppingCartProductId == id);
            if (shoppingCartProductModel == null)
            {
                return NotFound();
            }

            return View(shoppingCartProductModel);
        }

        // GET: ShoppingCartProductModels/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.ProductsModel, "ProductId", "ProductDescription");
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCartModel, "ShoppingCartId", "ShoppingCartId");
            return View();
        }

        // POST: ShoppingCartProductModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShoppingCartProductId,ShoppingCartId,ProductId,Quantity")] ShoppingCartProductModel shoppingCartProductModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCartProductModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.ProductsModel, "ProductId", "ProductDescription", shoppingCartProductModel.ProductId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCartModel, "ShoppingCartId", "ShoppingCartId", shoppingCartProductModel.ShoppingCartId);
            return View(shoppingCartProductModel);
        }

        // GET: ShoppingCartProductModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShoppingCartProductModel == null)
            {
                return NotFound();
            }

            var shoppingCartProductModel = await _context.ShoppingCartProductModel.FindAsync(id);
            if (shoppingCartProductModel == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.ProductsModel, "ProductId", "ProductDescription", shoppingCartProductModel.ProductId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCartModel, "ShoppingCartId", "ShoppingCartId", shoppingCartProductModel.ShoppingCartId);
            return View(shoppingCartProductModel);
        }

        // POST: ShoppingCartProductModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShoppingCartProductId,ShoppingCartId,ProductId,Quantity")] ShoppingCartProductModel shoppingCartProductModel)
        {
            if (id != shoppingCartProductModel.ShoppingCartProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCartProductModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartProductModelExists(shoppingCartProductModel.ShoppingCartProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.ProductsModel, "ProductId", "ProductDescription", shoppingCartProductModel.ProductId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCartModel, "ShoppingCartId", "ShoppingCartId", shoppingCartProductModel.ShoppingCartId);
            return View(shoppingCartProductModel);
        }

        // GET: ShoppingCartProductModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShoppingCartProductModel == null)
            {
                return NotFound();
            }

            var shoppingCartProductModel = await _context.ShoppingCartProductModel
                .Include(s => s.Product)
                .Include(s => s.ShoppingCart)
                .FirstOrDefaultAsync(m => m.ShoppingCartProductId == id);
            if (shoppingCartProductModel == null)
            {
                return NotFound();
            }

            return View(shoppingCartProductModel);
        }

        // POST: ShoppingCartProductModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShoppingCartProductModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShoppingCartProductModel'  is null.");
            }
            var shoppingCartProductModel = await _context.ShoppingCartProductModel.FindAsync(id);
            if (shoppingCartProductModel != null)
            {
                _context.ShoppingCartProductModel.Remove(shoppingCartProductModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartProductModelExists(int id)
        {
          return (_context.ShoppingCartProductModel?.Any(e => e.ShoppingCartProductId == id)).GetValueOrDefault();
        }
    }
}
