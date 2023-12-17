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
using MvcMovie.Services.EF;

namespace MvcMovie.Controllers
{
    public class RentalsController : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly RentalService _rentalService;
        private readonly RentalEFService _rentalEFService;

        public RentalsController(
            MvcMovieContext context,
            RentalService rentalService,
            RentalEFService rentalEFService
        )
        {
            _context = context;
            _rentalService = rentalService;
            _rentalEFService = rentalEFService;
        }


        // GET: Rentals
        // Original dbcontext method
        /*public async Task<IActionResult> Index()
        {
            var mvcMovieContext = _context.Rentals.Include(r => r.Movie).Include(r => r.User);
            return View(await mvcMovieContext.ToListAsync());
        }*/

        // Custom Rental service method
        public async Task<IActionResult> Index()
        {
            IEnumerable<Rental> rentals = await _rentalService.GetRentals();
            return View(rentals);
        }

        // raw sql method using FromSql of dbset
        /*public async Task<IActionResult> Index()
        {
            var mvcMovie = await _context.Rentals.FromSql($"SELECT [r].[RentalID], [r].[MovieId], [r].[RentEnd], [r].[RentStart], [r].[UserID], [m].[Title]\r\nFROM [Rentals] AS [r]\r\nINNER JOIN [Movie] AS [m] ON [r].[MovieId] = [m].[Id]\r\nINNER JOIN [User] AS [u] ON [r].[UserID] = [u].[UserID]").ToListAsync();
            return View(mvcMovie);
        }*/


        // GET: Rentals/Details/5
        // Original dbcontext method
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Movie)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }*/

        // Custom Rental service method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<Rental> rentals = await _rentalService.GetRentals(id);
            if (rentals == null)
            {
                return NotFound();
            }

            return View(rentals);
        }

        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var p1 = new SqlParameter("@Id", id);
            var rental = await _context.Rentals.FromSqlInterpolated($"SELECT TOP(1) [r].[RentalID], [r].[MovieId], [r].[RentEnd], [r].[RentStart], [r].[UserID], [m].[Title]\r\nFROM [Rentals] AS [r]\r\nINNER JOIN [Movie] AS [m] ON [r].[MovieId] = [m].[Id]\r\nINNER JOIN [User] AS [u] ON [r].[UserID] = [u].[UserID]\r\nWHERE [r].[RentalID] = {id}").FirstOrDefaultAsync();
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }*/


        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id");
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID");
            return View();
        }

        /*public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id");
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID");
            return View();
        }*/


        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalID,UserID,MovieId,RentStart,RentEnd")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(rental);
                //await _context.SaveChangesAsync();
                await _rentalService.AddRental(rental);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", rental.MovieId);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", rental.UserID);
            return View(rental);
        }

        /*public async Task<IActionResult> Create([Bind("RentalID,UserID,MovieId,RentStart,RentEnd")] Rental rental)
        {
            var insert
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", rental.MovieId);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", rental.UserID);
            return View(rental);
        }*/


        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", rental.MovieId);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", rental.UserID);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalID,UserID,MovieId,RentStart,RentEnd")] Rental rental)
        {
            if (id != rental.RentalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.RentalID))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", rental.MovieId);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", rental.UserID);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Movie)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.RentalID == id);
        }
    }
}
