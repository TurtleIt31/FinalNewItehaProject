using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalNewItehaProject.Data;
using FinalNewItehaProject.Models;

namespace FinalNewItehaProject.Controllers
{
    public class UserModelsController : Controller
    {

        public IActionResult ProtectedAction()
        {
            // Check if the session contains the UserId
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // If not authenticated, redirect to the login page
                return RedirectToAction("Login", "UserModels");
            }

            // User is authenticated, proceed with the action
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Login", "UserModels");
        }
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            // Hash the entered password
            var hashedPassword = HashPassword(enteredPassword);

            // Compare it with the stored hashed password
            return hashedPassword == storedHashedPassword;
        }
        private readonly ApplicationDbContext _context;

        public UserModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserModels
        public async Task<IActionResult> Index()
        {
            // Check if the user is an admin
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                // If the user is not an admin, redirect to an Access Denied page or home page
                return RedirectToAction("AccessDenied", "Home");
            }

            // If the user is an admin, continue to return the view
            return View(await _context.UserModel.ToListAsync());
        }

        // GET: UserModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserModel == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: UserModels/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: UserModels/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName,Password,Email,UserType")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                // Hash the password before saving (you'll need to implement password hashing)
                userModel.Password = HashPassword(userModel.Password);

                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: UserModels/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: UserModels/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user from the database based on the provided email
                var user = await _context.UserModel
                    .FirstOrDefaultAsync(u => u.Email == userModel.Email);

                if (user != null)
                {
                    // Verify the password
                    if (VerifyPassword(userModel.Password, user.Password))
                    {
                        // Store user information in session
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        HttpContext.Session.SetString("UserName", user.UserName);
                        HttpContext.Session.SetString("UserType", user.UserType);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Password was incorrect
                        ModelState.AddModelError(string.Empty, "The password you entered is incorrect.");
                    }
                }
                else
                {
                    // No user found with the provided email
                    ModelState.AddModelError(string.Empty, "No account found with that email.");
                }
            }

            return View(userModel);
        }

        // GET: UserModels/Create
        public IActionResult Create()
        {
            return View();
        }



        // POST: UserModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,Email,UserType")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: UserModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserModel == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: UserModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,Password,Email,UserType")] UserModel userModel)
        {
            if (id != userModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.UserId))
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
            return View(userModel);
        }

        // GET: UserModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserModel == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: UserModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserModel'  is null.");
            }
            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel != null)
            {
                _context.UserModel.Remove(userModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(int id)
        {
          return (_context.UserModel?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
