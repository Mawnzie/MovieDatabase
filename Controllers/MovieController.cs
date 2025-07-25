using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {   

        var movies = await _context.Movies
            .Include(m => m.Genre) // Add this line
            .ToListAsync();

        return View(movies);
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {

            List<SelectListItem> genres = new List<SelectListItem>();
            List<MovieGenre> possibleGenres = _context.Genres.ToList();
            foreach (var genre in possibleGenres)
            {
                genres.Add(new SelectListItem(genre.Type, genre.Id.ToString()));
            }
            ViewBag.SelectGenre = genres;
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Title,FileName,FileForm,File,GenreId,Year")] Movie movie)
        {
            if (movie.FileForm != null)
            {
                var memoryStream = new MemoryStream();
                movie.FileForm.CopyTo(memoryStream);
                movie.FileName = movie.FileForm.FileName;
                movie.File = memoryStream.ToArray();

            }
            
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage); // or log this
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdown list here when returning view with errors
            List<SelectListItem> genres = _context.Genres
                .Select(g => new SelectListItem
                {
                    Text = g.Type,
                    Value = g.Id.ToString()
                }).ToList();
            ViewBag.SelectGenre = genres;
                    
            return View(movie);
        }
        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            List<SelectListItem> genres = new List<SelectListItem>();
            List<MovieGenre> possibleGenres = _context.Genres.ToList();
            foreach (var genre in possibleGenres)
            {
                genres.Add(new SelectListItem(genre.Type, genre.Id.ToString()));
            }
            ViewBag.SelectGenre = genres;

            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,FileName,File,FileForm,GenreId,Year")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // If a new image was uploaded
                    if (movie.FileForm != null)
                    {
                        using var memoryStream = new MemoryStream();
                        await movie.FileForm.CopyToAsync(memoryStream);
                        movie.File = memoryStream.ToArray();
                        movie.FileName = movie.FileForm.FileName;
                    }
                    else
                    {
                        // Keep the existing image if no new file was uploaded
                        var existingMovie = await _context.Movies.AsNoTracking()
                            .FirstOrDefaultAsync(m => m.MovieId == id);
                        movie.File = existingMovie.File;
                        movie.FileName = existingMovie.FileName;
                    }

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            List<SelectListItem> genres = _context.Genres
                .Select(g => new SelectListItem
                {
                    Text = g.Type,
                    Value = g.Id.ToString()
                }).ToList();
            ViewBag.SelectGenre = genres;
            // ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Type", movie.GenreId);
            return View(movie);

        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
