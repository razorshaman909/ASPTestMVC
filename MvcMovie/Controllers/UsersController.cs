using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.Services;

namespace MvcMovie.Controllers
{
    public class UsersController : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly UserService _userService;

        public UsersController(
            MvcMovieContext context,
            UserService userService
            )
        {
            _context = context;
            _userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {

            /*var users = _context.Users.Include(u => u.Rentals).ToList();*/
            var users = await _userService.GetUsers();

            foreach (var user in users)
            {
                user.RentalStatus = GetRentalStatus(user.Rentals) ;
            }

            return View(users);
        }



        private string GetRentalStatus(ICollection<Rental> rentals)
        {
            
            if (rentals == null || rentals.Count == 0)
            {
                return "No rental";
            }

            var hasOverdueRental = rentals.Any(r => r.RentEnd < DateTime.Now);

            return hasOverdueRental ? "Overdue" : "Renting";
        }

        /*private string GetRentalStatus(ICollection<Rental> rentals)
        {
            if (rentals == null || rentals.Count == 0)
            {
                return "No rental";
            }

            var hasOverdueRental = rentals.Any(r => r.RentEnd < DateTime.Now);

            return hasOverdueRental ? "Overdue" : "Renting";
        }*/

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _userService.GetUsers(id);
            /*var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);*/
            if (users == null)
            {
                return NotFound();
            }

            foreach (var user in users)
            {
                user.RentalStatus = (user.Rentals == null || user.Rentals.Count == 0)
                    ? "No Rental"
                    : user.Rentals.Any(r => r.RentEnd < DateTime.Now) ? "Overdue" : "Renting";
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,LastName,FirstMidName,JoinDate")] User user)
        {
            if (ModelState.IsValid)
            {
                /*_context.Add(user);
                await _context.SaveChangesAsync();*/
                await _userService.AddUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,LastName,FirstMidName,JoinDate")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*_context.Update(user);
                    await _context.SaveChangesAsync();*/
                    await _userService.UpdateUser(id, user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);*/
            var users = await _userService.GetUsers(id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();*/
            await _userService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
