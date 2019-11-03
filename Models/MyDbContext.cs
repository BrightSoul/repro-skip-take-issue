using Microsoft.EntityFrameworkCore;

namespace SkipTakeRepro.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.OwnsOne(course => course.CurrentPrice, builder => {
                    builder.Property(money => money.Currency).HasConversion<string>();
                });
            });
        }
    }
}