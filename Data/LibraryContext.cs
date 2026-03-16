using Microsoft.EntityFrameworkCore;
using LibraryManagement.Models;
using System;

namespace LibraryManagement.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=LibraryDB;Trusted_Connection=True;MultipleActiveResultSets=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                
                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(b => b.ISBN)
                    .HasMaxLength(20);
                
                entity.Property(b => b.PublishYear)
                    .IsRequired();
                
                entity.Property(b => b.QuantityInStock)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Genre)
                    .WithMany(g => g.Books)
                    .HasForeignKey(b => b.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);
                
                entity.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(a => a.Country)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id);
                
                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(g => g.Description)
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9), Country = "Россия" },
                new Author { Id = 2, FirstName = "Федор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11), Country = "Россия" }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Роман", Description = "Художественное произведение" },
                new Genre { Id = 2, Name = "Детектив", Description = "Детективный жанр" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Война и мир", AuthorId = 1, GenreId = 1, PublishYear = 1869, ISBN = "978-5-17-123456-7", QuantityInStock = 5 },
                new Book { Id = 2, Title = "Преступление и наказание", AuthorId = 2, GenreId = 1, PublishYear = 1866, ISBN = "978-5-17-765432-1", QuantityInStock = 3 }
            );
        }
    }
}