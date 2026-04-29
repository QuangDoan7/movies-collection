using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using movies_collection.Models;

namespace movies_collection.Pages.Movies
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly movies_collection.Data.MoviesDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(movies_collection.Data.MoviesDbContext context, IWebHostEnvironment environment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        [BindProperty]
        public List<string> SelectedGenres { get; set; } = new();

        private void LoadGenres()
        {
            var genres = new[] { "Action", "Animation", "Comedy", "Crime", "Documentary", "Drama", "Fantasy", "Horror", "Music", "Romance", "Sci-Fi", "Sports", "Thriller", "Western" };

            ViewData["Genres"] = new MultiSelectList(genres);
        }

        public IActionResult OnGet()
        {
            LoadGenres();

            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userID = _userManager.GetUserId(User);
            if (userID == null)
            {
                return Challenge();
            }

            Movie.Genre = string.Join(", ", SelectedGenres);
            Movie.UserId = userID;

            if (!ModelState.IsValid)
            {
                LoadGenres();
                return Page();
            }

            if (Upload != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "movieimg");

                Directory.CreateDirectory(uploadsFolder);

                var safeMovieName = Movie.Title.Replace(" ", "-").ToLower();

                var fileName = $"{safeMovieName}_{Guid.NewGuid()}{Path.GetExtension(Upload.FileName)}";

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }

                Movie.MoviePosterUrl = fileName;
            }

            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
