namespace SocialNetworkDb.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using SocialNetworkDb.Model;
    public class SocialNetworkDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=SocialNetwork;Integrated Security = true;");
            }
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

    }
}
