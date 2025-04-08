using AuctionsApi_v2.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Data.DataModels
{
    public class AuctionDbContext: DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Auctions> Auctions { get; set; }
        public DbSet<Bids> Bids { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("Categories");

                entity.Property(c => c.CategoryId)
                    .IsRequired()
                    .UseIdentityColumn(1,1)
                    .HasColumnName("CategoryId");

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasColumnName("Name")
                    .HasMaxLength(50);

                entity.Property(c => c.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(100);

                entity.Property(c => c.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETDATE()");

                entity.HasKey(c => c.CategoryId);
            });


            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(u => u.UserId)
                    .IsRequired()
                    .UseIdentityColumn(1,1)
                    .HasColumnName("UserId");

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasColumnName("Username")
                    .HasMaxLength(50);

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasColumnName("Password")
                    .HasMaxLength(255);

                entity.Property(u => u.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(u => u.LastLogin)
                    .HasColumnName("LastLogin");

                entity.HasKey(u => u.UserId);

                entity.HasIndex(u => u.Username)
                    .IsUnique();
            });


            modelBuilder.Entity<Auctions>(entity =>
            {
                entity.ToTable("Auctions");

                entity.Property(a => a.AuctionId)
                    .IsRequired()
                    .UseIdentityColumn(1,1)
                    .HasColumnName("AuctionId");

                entity.Property(a => a.Title)
                    .IsRequired()
                    .HasColumnName("Title")
                    .HasMaxLength(50);

                entity.Property(a => a.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(250);

                entity.Property(a => a.StartingPrice)
                    .IsRequired()
                    .HasColumnName("StartingPrice")
                    .HasColumnType("decimal(19,3)");

                entity.Property(a => a.OpeningTime)
                    .HasColumnName("OpeningTime");

                entity.Property(a => a.ClosingTime)
                    .HasColumnName("ClosingTime");


                entity.Property(a => a.HighestBid)
                    .HasColumnName("HighestBid")
                    .HasColumnType("decimal(19,3)")
                    .HasDefaultValue(0.00);

                entity.Property(a => a.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(a => a.CategoryId)
                    .IsRequired()
                    .HasColumnName("CategoryId");

                entity.Property(a => a.UserId)
                    .IsRequired()
                    .HasColumnName("UserId");

                entity.HasKey(a => a.AuctionId);

                entity.HasOne(a => a.Category)
                    .WithMany(c => c.Auctions)
                    .HasForeignKey(a => a.CategoryId);

                entity.HasOne(a => a.User)
                    .WithMany(u => u.Auctions)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasCheckConstraint("CHK_ValidAuctionTimes", "ClosingTime > OpeningTime");
                entity.HasCheckConstraint("CHK_FutureOpeningTime", "OpeningTime > GETDATE()");


            });

            modelBuilder.Entity<Bids>(entity =>
            {
                entity.ToTable("Bids");

                entity.Property(b => b.BidId)
                    .IsRequired()
                    .UseIdentityColumn(1,1)
                    .HasColumnName("BidId");

                entity.Property(b => b.BidAmount)
                    .IsRequired()
                    .HasColumnName("BidAmount")
                    .HasColumnType("decimal(19,3)");

                entity.Property(b => b.BidTime)
                    .HasColumnName("BidTime")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(b => b.UserId)
                    .IsRequired()
                    .HasColumnName("UserId");

                entity.Property(b => b.AuctionId)
                    .IsRequired()
                    .HasColumnName("AuctionId");

                entity.Property(b => b.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETDATE()");

                entity.HasKey(b => b.BidId);

                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bids)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Auction)
                    .WithMany(a => a.Bids)
                    .HasForeignKey(b => b.AuctionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasCheckConstraint("CHK_ValidBidAmount", "BidAmount > 0");
            });
        }
    }
}
