using Microsoft.EntityFrameworkCore;
using ProjektyElektronika.Api.Models;

namespace ProjektyElektronika.Api.DateBase
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
