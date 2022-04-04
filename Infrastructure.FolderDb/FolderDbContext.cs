using Domains.Entities.FolderDbModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.FolderDb
{
    public class FolderDbContext : DbContext
    {
        public FolderDbContext(DbContextOptions<FolderDbContext> options) : base(options)
        {
        }

        public DbSet<Folders> Folders { get; set; }
        public DbSet<Files> Files { get; set; }
    }
}
