using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using movies_collection.Models;

namespace movies_collection.Pages.Movies
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly movies_collection.Data.MoviesDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;


        public EditModel(movies_collection.Data.MoviesDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [BindProperty]
        public List<string> SelectedGenres { get; set; } = new();

        private void LoadGenres()
        {
            var genres = new[] { "Action", "Animation", "Comedy", "Crime", "Documentary", "Drama", "Fantasy", "Horror", "Music", "Romance", "Sci-Fi", "Sports", "Thriller", "Western" };

            ViewData["Genres"] = new MultiSelectList(genres);
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        [BindProperty]
        public IFormFile? Upload { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie =  await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }
            Movie = movie;

            SelectedGenres = Movie.Genre?
                .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                .ToList() ?? new List<string>();
            
            LoadGenres();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            Movie.Genre = string.Join(", ", SelectedGenres);
            Movie.UserId = userId;

            if (!ModelState.IsValid)
            {
                LoadGenres();
                return Page();
            }

            if (Upload != null)
            {
                if (!string.IsNullOrEmpty(Movie.MoviePosterUrl))
                {
                    var oldFileName = Path.GetFileName(Movie.MoviePosterUrl);

                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "movieimg", oldFileName);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "movieimg");

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

            _context.Attach(Movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.MovieId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
