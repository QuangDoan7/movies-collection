using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using movies_collection.Models;

namespace movies_collection.Pages.Movies
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly movies_collection.Data.MoviesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(movies_collection.Data.MoviesDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string TitleSort { get; set; }
        public string ReleaseYearSort { get; set; }
        public string CurrentSort { get; set; }


        public IList<Movie> Movie { get;set; } = default!;

        public List<string> Genres { get; set; } = new List<string>();
        public string? CurrentGenre { get; set; }

        public async Task OnGetAsync(string? sortOrder, string? genre)
        {
            CurrentSort = sortOrder;
            CurrentGenre = genre;

            TitleSort = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ReleaseYearSort = sortOrder == "year" ? "year_desc" : "year";

            var userId = _userManager.GetUserId(User);

            var allMovies = await _context.Movies
                .Where(m => m.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            Genres = allMovies
                .SelectMany(m => m.Genre.Split(", ", StringSplitOptions.RemoveEmptyEntries))
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            if (!string.IsNullOrEmpty(genre))
            {
                allMovies = allMovies
                    .Where(m => m.Genre.Split(", ", StringSplitOptions.RemoveEmptyEntries)
                    .Contains(genre))
                    .ToList();
            }

            switch (sortOrder)
            {
                case "title_desc":
                    allMovies = allMovies.OrderByDescending(m => m.Title.ToLower()).ToList();
                    break;
                case "year":
                    allMovies = allMovies.OrderBy(m => m.ReleaseYear).ToList();
                    break;
                case "year_desc":
                    allMovies = allMovies.OrderByDescending(m => m.ReleaseYear).ToList();
                    break;
                default:
                    allMovies = allMovies.OrderBy(m => m.Title.ToLower()).ToList();
                    break;
            }

            Movie = allMovies;
        }
    }
}
