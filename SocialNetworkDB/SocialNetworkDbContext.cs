﻿using Microsoft.EntityFrameworkCore;
using SocialNetworkDB.Model;

namespace SocialNetworkDb
{
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
