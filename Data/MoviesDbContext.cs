using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using movies_collection.Models;

namespace movies_collection.Data;

public class MoviesDbContext : IdentityDbContext
{
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
    {
    }
    public DbSet<Movie> Movies { get; set; }
}
