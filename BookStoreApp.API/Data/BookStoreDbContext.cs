using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
{
   public BookStoreDbContext()
   {
   }

   public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
       : base(options)
   {
   }

   public virtual DbSet<Author> Authors { get; set; }

   public virtual DbSet<Book> Books { get; set; }


   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<Author>(entity =>
      {
         entity.Property(e => e.Bio).HasMaxLength(250);
         entity.Property(e => e.FirstName).HasMaxLength(50);
         entity.Property(e => e.LastName).HasMaxLength(50);
      });

      modelBuilder.Entity<Book>(entity =>
      {
         entity.Property(e => e.Image).HasMaxLength(50);
         entity.Property(e => e.Isbn)
               .HasMaxLength(50)
               .HasColumnName("ISBN");
         entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
         entity.Property(e => e.Summary).HasMaxLength(250);
         entity.Property(e => e.Title).HasMaxLength(50);

         entity.HasOne(d => d.Author).WithMany(p => p.Books)
               .HasForeignKey(d => d.AuthorId)
               .HasConstraintName("FK_Books_Authors");
      });

      modelBuilder.Entity<IdentityRole>().HasData(
         new IdentityRole
         {
            Name= "User",
            NormalizedName = "USER",
            Id = "1f405c02-2d7e-4f1b-8fe0-0a72615a1195"
         },
         new IdentityRole
         {
            Name= "Administrator",
            NormalizedName = "ADMINISTRATOR",
            Id = "079f8246-fc0c-47a3-bc46-ff2bcd833ae4"
         }
      );

      var hasher = new PasswordHasher<ApiUser>();

      modelBuilder.Entity<ApiUser>().HasData(
         new ApiUser
         {
            Id = "c8b4522a-6024-4c5d-bad4-12fb0fb0028d",
            Email = "admin@bookstore.com",
            NormalizedEmail = "ADMIN@BOOKSTORE.COM",
            UserName = "admin@bookstore.com",
            NormalizedUserName = "ADMIN@BOOKSTORE.COM",
            FirstName = "System",
            LastName = "Admin",
            PasswordHash = hasher.HashPassword(null, "P@ssword1")
         },
         new ApiUser
         {
            Id = "5fd7beae-40b3-45f2-8dd6-8eebd57feaec",
            Email = "user@bookstore.com",
            NormalizedEmail = "USER@BOOKSTORE.COM",
            UserName = "user@bookstore.com",
            NormalizedUserName = "USER@BOOKSTORE.COM",
            FirstName = "System",
            LastName = "User",
            PasswordHash = hasher.HashPassword(null, "P@ssword1")
         }
         );

      modelBuilder.Entity<IdentityUserRole<string>>().HasData(
         new IdentityUserRole<string>
         {
            RoleId = "1f405c02-2d7e-4f1b-8fe0-0a72615a1195",
            UserId = "5fd7beae-40b3-45f2-8dd6-8eebd57feaec"
         },
         new IdentityUserRole<string>
         {
            RoleId = "079f8246-fc0c-47a3-bc46-ff2bcd833ae4",
            UserId = "c8b4522a-6024-4c5d-bad4-12fb0fb0028d"
         }
         );

      OnModelCreatingPartial(modelBuilder);
   }

   partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
