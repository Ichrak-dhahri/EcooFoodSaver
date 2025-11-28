// Data/EcoFoodDbContext.cs
using EcoFoodAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoFoodAPI.Data
{
    public class EcoFoodDbContext : DbContext
    {
        public EcoFoodDbContext(DbContextOptions<EcoFoodDbContext> options)
            : base(options)
        {
        }

        // DbSets pour toutes vos tables
        public DbSet<User> Users { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Categorie> Categories { get; set; } // Si vous avez cette table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la table User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
                entity.HasKey(e => e.IdUser);
                entity.Property(e => e.IdUser).ValueGeneratedOnAdd();

                entity.Property(e => e.Nom).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Prenom).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.MotDePasse).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Ville).HasMaxLength(100);
                entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValue("User");

                entity.Property(e => e.MoyenneRating).HasColumnType("decimal(3,2)");
                entity.Property(e => e.TotalRatingsRecus).HasDefaultValue(0);
                entity.Property(e => e.TotalRatingsDonnes).HasDefaultValue(0);

                // Index unique sur l'email
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuration de la table Produit
            modelBuilder.Entity<Produit>(entity =>
            {
                entity.ToTable("produit"); // Nom exact de votre table (singulier)
                entity.HasKey(e => e.IdProduit);
                entity.Property(e => e.IdProduit).ValueGeneratedOnAdd();

                entity.Property(e => e.Titre).IsRequired();
                entity.Property(e => e.Prix).HasColumnType("decimal(10,2)");

                // Relations
                entity.HasOne(p => p.IdUserNavigation)
                      .WithMany(u => u.Produits)
                      .HasForeignKey(p => p.IdUser)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.IdCategorieNavigation)
                      .WithMany(c => c.Produits)
                      .HasForeignKey(p => p.IdCategorie)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuration de la table Message
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("message"); // ✅ CORRECTION: message, pas reservation !
                entity.HasKey(e => e.IdMessage);
                entity.Property(e => e.IdMessage).ValueGeneratedOnAdd();

                // Relations avec User
                entity.HasOne(m => m.IdSenderNavigation)
                      .WithMany(u => u.MessageIdSenderNavigations)
                      .HasForeignKey(m => m.IdSender)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.IdReceiverNavigation)
                      .WithMany(u => u.MessageIdReceiverNavigations)
                      .HasForeignKey(m => m.IdReceiver)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relation avec Produit (si applicable)
                entity.HasOne(m => m.IdProduitNavigation)
                      .WithMany(p => p.Messages)
                      .HasForeignKey(m => m.IdProduit)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration de la table Rating
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("rating");
                entity.HasKey(e => e.IdRating);
                entity.Property(e => e.IdRating).ValueGeneratedOnAdd();

                // Relation avec User (FromUser)
                entity.HasOne(r => r.IdFromUserNavigation)
                      .WithMany(u => u.RatingIdFromUserNavigations)
                      .HasForeignKey(r => r.IdFromUser)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relation avec User (ToUser)
                entity.HasOne(r => r.IdToUserNavigation)
                      .WithMany(u => u.RatingIdToUserNavigations)
                      .HasForeignKey(r => r.IdToUser)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuration de la table Reservation
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("reservation"); // ✅
                entity.HasKey(e => e.IdReservation);
                entity.Property(e => e.IdReservation).ValueGeneratedOnAdd();

                // Relations
                entity.HasOne(r => r.IdUserNavigation)
                      .WithMany(u => u.Reservations)
                      .HasForeignKey(r => r.IdUser)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.IdProduitNavigation)
                      .WithMany(p => p.Reservations)
                      .HasForeignKey(r => r.IdProduit)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}