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
    public class ProductsModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductsModels
        public async Task<IActionResult> Index()
        {
              return _context.ProductsModel != null ? 
                          View(await _context.ProductsModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ProductsModel'  is null.");
        }

        // GET: ProductsModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductsModel == null)
            {
                return NotFound();
            }

            var productsModel = await _context.ProductsModel
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productsModel == null)
            {
                return NotFound();
            }

            return View(productsModel);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View();
        }

        // POST: ProductsModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductName,ProductDescription,NumberOfProducts,ProductImage,ProductPrice")] ProductsModel productsModel)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Add(productsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productsModel);
        }

        // GET: ProductsModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductsModel == null)
            {
                return NotFound();
            }

            var productsModel = await _context.ProductsModel.FindAsync(id);
            if (productsModel == null)
            {
                return NotFound();
            }
            return View(productsModel);
        }

        // POST: ProductsModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,NumberOfProducts,ProductImage,ProductPrice")] ProductsModel productsModel)
        {
            if (id != productsModel.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsModelExists(productsModel.ProductId))
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
            return View(productsModel);
        }

        // GET: ProductsModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductsModel == null)
            {
                return NotFound();
            }

            var productsModel = await _context.ProductsModel
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productsModel == null)
            {
                return NotFound();
            }

            return View(productsModel);
        }

        // POST: ProductsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductsModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProductsModel'  is null.");
            }
            var productsModel = await _context.ProductsModel.FindAsync(id);
            if (productsModel != null)
            {
                _context.ProductsModel.Remove(productsModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsModelExists(int id)
        {
          return (_context.ProductsModel?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
