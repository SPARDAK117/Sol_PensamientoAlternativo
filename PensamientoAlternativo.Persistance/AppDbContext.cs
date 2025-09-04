using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Domain.Entities;
using PensamientoAlternativo.Domain.Entities.Auth;
using PensamientoAlternativo.Domain.Entities.Blog;
using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Persistance
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ClientSettings> ClientSettings => Set<ClientSettings>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<ContactForm> ContactForms => Set<ContactForm>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Video> Videos => Set<Video>();
        public DbSet<Opinion> Opinions => Set<Opinion>();
        public DbSet<Faq> Faqs => Set<Faq>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
        public DbSet<LoginCredentials> LoginCredentials => Set<LoginCredentials>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
                entity.Property(c => c.IsEmailNotificationsAvailable).IsRequired();
                entity.Property(c => c.AcceptTermsAndConditions).IsRequired();
            });

            modelBuilder.Entity<ContactForm>(entity =>
            {
                entity.Property(f => f.Message).IsRequired();
                entity.Property(f => f.Message).HasMaxLength(500);
            });

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.ContactForms)
                .WithOne(f => f.Customer)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Opinions)
                .WithOne(f => f.Customer)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoginCredentials>().Property(x => x.UserEmail).IsRequired();
            modelBuilder.Entity<LoginCredentials>().Property(x => x.Password).IsRequired();

            modelBuilder.Entity<Image>().Property(x => x.IsBannerImage).IsRequired();
            modelBuilder.Entity<Image>().Property(x => x.IsVisible).IsRequired();
            modelBuilder.Entity<Image>().Property(x => x.ViewSection).IsRequired();
            modelBuilder.Entity<Image>().Property(x => x.Title).IsRequired();
            modelBuilder.Entity<Image>().Property(i => i.Url).IsRequired();
            modelBuilder.Entity<Image>().Property(x => x.Description).IsRequired();

            modelBuilder.Entity<Service>().Property(x => x.IconName).IsRequired();
            modelBuilder.Entity<Service>().Property(x => x.IconPath).IsRequired();
            modelBuilder.Entity<Service>().Property(x => x.Title).IsRequired();
            modelBuilder.Entity<Service>().Property(x => x.Subtitle).IsRequired();

            modelBuilder.Entity<Video>().Property(x => x.Title).IsRequired();
            modelBuilder.Entity<Video>().Property(x => x.Description).IsRequired();
            modelBuilder.Entity<Video>().Property(x => x.Url).IsRequired();
            modelBuilder.Entity<Video>().Property(x => x.IsVisible).IsRequired();
            modelBuilder.Entity<Video>().Property(x => x.ViewSection).IsRequired();


            modelBuilder.Entity<Opinion>().Property(x => x.AuthorName).IsRequired();
            modelBuilder.Entity<Opinion>().Property(x => x.StarRate).IsRequired();
            modelBuilder.Entity<Opinion>().Property(x => x.OpinionText).IsRequired();
            modelBuilder.Entity<Opinion>().Property(x => x.IsVisible).IsRequired();

            modelBuilder.Entity<Faq>().Property(x => x.Question).IsRequired();
            modelBuilder.Entity<Faq>().Property(x => x.Answer).IsRequired();
            modelBuilder.Entity<Faq>().Property(x => x.IsVisible).IsRequired();


            modelBuilder.Entity<BlogCategory>(entity =>
            {
                entity.Property(c => c.Name).IsRequired();
                entity.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(a => a.Title).IsRequired();
                entity.Property(a => a.Description).IsRequired();
                entity.Property(a => a.ImageUrl).IsRequired();
                entity.Property(a => a.CreatedDate).IsRequired();

                entity.HasOne(a => a.Category)
                    .WithMany(c => c.Articles)
                    .HasForeignKey(a => a.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }


    }
}
