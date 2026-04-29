using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace movies_collection.Models;

public class Movie
{
    [Key]
    public int MovieId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Release Year")]
    public int? ReleaseYear { get; set; }

    public int? Duration { get; set; }

    [Display(Name = "Last Watched Year")]
    [LastWatchedYearValidation]
    public int? LastWatchedYear { get; set; }

    public string Genre { get; set; } = string.Empty;

    [Display(Name = "Personal Rating")]
    [Range(1, 100, ErrorMessage = "Rating must be between 1 and 100.")]
    public int? Rating { get; set; }

    [Display(Name = "Movie Description")]
    public string? Description { get; set; } = string.Empty;

    [Display(Name = "Movie Poster")]
    public string? MoviePosterUrl { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public string? UserId { get; set; }

    public IdentityUser? User { get; set; }
}
