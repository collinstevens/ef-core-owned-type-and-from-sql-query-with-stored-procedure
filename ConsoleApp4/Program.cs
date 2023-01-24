using ConsoleApp4;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

var applicationDbContext = new ApplicationDbContext();

await applicationDbContext.Blogs.FromSqlInterpolated($"EXECUTE sp_GetBlogs").ToListAsync();

namespace ConsoleApp4
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Blog> Blogs => Set<Blog>();
    }
}

public class Blog
{
    public int Id { get; set; }

    public Box Box { get; set; }
}

public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.OwnsOne<Box>(b => b.Box);
    }
}

public class Box
{
    public int Value { get; set; }
}