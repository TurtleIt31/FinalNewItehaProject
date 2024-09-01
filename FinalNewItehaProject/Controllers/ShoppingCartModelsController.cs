using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalNewItehaProject.Data;
using ITEHA_Project.Models;

namespace FinalNewItehaProject.Controllers
{
    public class ShoppingCartModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCartModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShoppingCartModel.Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShoppingCartModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShoppingCartModel == null)
            {
                return NotFound();
            }

            var shoppingCartModel = await _context.ShoppingCartModel
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.ShoppingCartId == id);
            if (shoppingCartModel == null)
            {
                return NotFound();
            }

            return View(shoppingCartModel);
        }

        // GET: ShoppingCartModels/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.UserModel, "UserId", "Email");
            return View();
        }

        // POST: ShoppingCartModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShoppingCartId,UserId")] ShoppingCartModel shoppingCartModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCartModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.UserModel, "UserId", "Email", shoppingCartModel.UserId);
            return View(shoppingCartModel);
        }

        // GET: ShoppingCartModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShoppingCartModel == null)
            {
                return NotFound();
            }

            var shoppingCartModel = await _context.ShoppingCartModel.FindAsync(id);
            if (shoppingCartModel == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.UserModel, "UserId", "Email", shoppingCartModel.UserId);
            return View(shoppingCartModel);
        }

        // POST: ShoppingCartModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShoppingCartId,UserId")] ShoppingCartModel shoppingCartModel)
        {
            if (id != shoppingCartModel.ShoppingCartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCartModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartModelExists(shoppingCartModel.ShoppingCartId))
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
            ViewData["UserId"] = new SelectList(_context.UserModel, "UserId", "Email", shoppingCartModel.UserId);
            return View(shoppingCartModel);
        }

        // GET: ShoppingCartModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShoppingCartModel == null)
            {
                return NotFound();
            }

            var shoppingCartModel = await _context.ShoppingCartModel
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.ShoppingCartId == id);
            if (shoppingCartModel == null)
            {
                return NotFound();
            }

            return View(shoppingCartModel);
        }

        // POST: ShoppingCartModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShoppingCartModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShoppingCartModel'  is null.");
            }
            var shoppingCartModel = await _context.ShoppingCartModel.FindAsync(id);
            if (shoppingCartModel != null)
            {
                _context.ShoppingCartModel.Remove(shoppingCartModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartModelExists(int id)
        {
          return (_context.ShoppingCartModel?.Any(e => e.ShoppingCartId == id)).GetValueOrDefault();
        }
    }
}
