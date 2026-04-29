using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using movies_collection.Models;

namespace movies_collection.Pages.Movies
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly movies_collection.Data.MoviesDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(movies_collection.Data.MoviesDbContext context, IWebHostEnvironment environment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie is not null)
            {
                Movie = movie;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Where(m => m.UserId == userId)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            
            if (movie != null)
            {
                Movie = movie;
                _context.Movies.Remove(Movie);
                await _context.SaveChangesAsync();

                if (movie.MoviePosterUrl != null)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "movieimg", movie.MoviePosterUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
