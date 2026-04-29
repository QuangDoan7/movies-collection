using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using movies_collection.Models;

namespace movies_collection.Pages.Movies
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly movies_collection.Data.MoviesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(movies_collection.Data.MoviesDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Movie Movie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userID = _userManager.GetUserId(User);

            if (userID == null) {
                return Challenge();
            }

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
    }
}
