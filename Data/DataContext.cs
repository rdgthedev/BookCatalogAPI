using BookCatalog.Data.Mappings;
using BookCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Data
{
    public class DataContext : DbContext
    {
        private IConfiguration _configutarion;
        public DataContext(IConfiguration configuration) 
        {
            _configutarion = configuration;
        }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_configutarion.GetConnectionString("DefaultConnection"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
        }
    }
}
