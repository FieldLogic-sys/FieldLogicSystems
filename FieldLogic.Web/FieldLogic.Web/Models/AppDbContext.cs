using Microsoft.EntityFrameworkCore;
namespace FieldLogic.Web.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    
        public DbSet<MediaEntry> MediaEntries { get; set;  }
    }
