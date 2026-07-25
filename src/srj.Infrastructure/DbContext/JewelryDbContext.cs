using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using srj.Domain.Models;
using srj.Infrastructure.Identity;

namespace srj.Infrastructure.DbContext;

public class JewelryDbContext : IdentityDbContext<ApplicationUser>
{
    public JewelryDbContext(
        DbContextOptions<JewelryDbContext> options)
        : base(options)
    {
    }

    public DbSet<JewelryItem> JewelryItems { get; set; }
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<GoldPrice> GoldPrices { get; set; }
    public DbSet<SilverPrice> SilverPrices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --------------------------
        // Item Category
        // --------------------------
        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Metal).HasConversion<string>().HasMaxLength(10);
            entity.HasIndex(e => new { e.Metal, e.Name }).IsUnique();
        });

        // --------------------------
        // Jewelry Item
        // --------------------------
        modelBuilder.Entity<JewelryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Sku).IsUnique();
            entity.Property(e => e.Sku).HasMaxLength(50).IsRequired();
            entity.Property(e => e.WeightInGrams).HasColumnType("decimal(10,3)");
            entity.Property(e => e.StoneWeight).HasColumnType("decimal(10,3)");
            entity.Property(e => e.StonePrice).HasColumnType("decimal(10,2)");
            entity.Property(e => e.PurityInPercentage).HasColumnType("decimal(5,2)");
            entity.Property(e => e.MakingChargePercentage).HasColumnType("decimal(10,2)");
            entity.Property(e => e.MakingChargeWeight).HasColumnType("decimal(10,2)");
            entity.Property(e => e.HallMarkCharge).HasColumnType("decimal(10,2)");

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --------------------------
        // Gold Price
        // --------------------------
        modelBuilder.Entity<GoldPrice>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.PriceDate)
                .IsUnique();

            entity.Property(x => x.SellPrice24KImp).HasPrecision(18, 2);
            entity.Property(x => x.BuyPrice24KImp).HasPrecision(18, 2);

            entity.Property(x => x.SellPrice24KFt).HasPrecision(18, 2);
            entity.Property(x => x.BuyPrice24KFt).HasPrecision(18, 2);

            entity.Property(x => x.SellPrice22K).HasPrecision(18, 2);
            entity.Property(x => x.SellPrice20K).HasPrecision(18, 2);
            entity.Property(x => x.SellPrice18K).HasPrecision(18, 2);
        });

        // --------------------------
        // Silver Price
        // --------------------------
        modelBuilder.Entity<SilverPrice>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.PriceDate)
                .IsUnique();

            entity.Property(x => x.SellPriceSilly).HasPrecision(18, 2);
            entity.Property(x => x.BuyPriceSilly).HasPrecision(18, 2);

            entity.Property(x => x.SellPrice99).HasPrecision(18, 2);
            entity.Property(x => x.BuyPrice99).HasPrecision(18, 2);
        });
    }
}