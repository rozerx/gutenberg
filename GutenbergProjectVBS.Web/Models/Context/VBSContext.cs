using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GutenbergProjectVBS.Web.Models.Context
{
    public class VBSContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Library> Libraries { get; set; }

        public DbSet<BookPage> BookPages { get; set; }

        public VBSContext()
        {
            Database.SetInitializer(new VBSInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasMany<Author>(b => b.Authors).WithMany(a => a.Books).Map(ab => {
                ab.MapLeftKey("BookRefId");
                ab.MapRightKey("AuthorRefId");
                ab.ToTable("BookAuthor");
            });

            modelBuilder.Entity<Book>().HasMany<Category>(b => b.Categories).WithMany(c => c.Books).Map(cb => {
                cb.MapLeftKey("BookRefId");
                cb.MapRightKey("CategoryRefId");
                cb.ToTable("BookCategory");
            });

            modelBuilder.Entity<Book>().HasMany<Language>(b => b.Languages).WithMany(l => l.Books).Map(lb => {
                lb.MapLeftKey("BookRefId");
                lb.MapRightKey("LanguageRefId");
                lb.ToTable("BookLanguage");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}