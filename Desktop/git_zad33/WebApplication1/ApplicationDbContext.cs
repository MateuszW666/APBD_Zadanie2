using Microsoft.EntityFrameworkCore;
using AnimalsApi.Models;

namespace AnimalsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet Animal>Animals {get;set }