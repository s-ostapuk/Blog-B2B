﻿using Blog_Server.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Blog_Server.Models.Database
{
    public class ApplicationDbContext : DbContext
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
