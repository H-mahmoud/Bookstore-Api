using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {
        }

        public DbSet<BookModel> Book { get; set; }
        public DbSet<AuthorModel> Author { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<JwtTokensModel> JwtToken { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BookModel>()
                .HasOne(x => x.Author)
                .WithMany(y => y.Books)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<BookModel>()
                .HasOne(x => x.Category)
                .WithMany(y => y.Books)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<UserModel>()
                .HasIndex(x => x.Email)
                .IsUnique();

            base.OnModelCreating(builder);
        }

    }
}
