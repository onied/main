using Microsoft.EntityFrameworkCore;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Purchase> Purchases { get; set; } = null!;
    public DbSet<PurchaseDetails> PurchaseDetails { get; set; } = null!;
    public DbSet<CoursePurchaseDetails> CoursePurchaseDetails { get; set; } = null!;
    public DbSet<CertificatePurchaseDetails> CertificatePurchaseDetails { get; set; } = null!;
    public DbSet<SubscriptionPurchaseDetails> SubscriptionPurchaseDetails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Purchase>()
            .HasOne<User>(p => p.User)
            .WithMany(u => u.Purchases)
            .HasForeignKey(p => p.UserId);
        modelBuilder.Entity<Purchase>()
            .HasOne<PurchaseDetails>(p => p.PurchaseDetails)
            .WithOne()
            .HasForeignKey<Purchase>(p => p.PurchaseDetails)
            .HasForeignKey<PurchaseDetails>(pd => pd.Id);

        modelBuilder.Entity<PurchaseDetails>()
            .HasDiscriminator(pd => pd.PurchaseType)
            .HasValue<CoursePurchaseDetails>(PurchaseType.Course)
            .HasValue<CertificatePurchaseDetails>(PurchaseType.Certificate)
            .HasValue<SubscriptionPurchaseDetails>(PurchaseType.Subscription);
        modelBuilder.Entity<CoursePurchaseDetails>()
            .HasOne<Course>(pd => pd.Course)
            .WithMany()
            .HasForeignKey(pd => pd.CourseId);
        modelBuilder.Entity<CertificatePurchaseDetails>()
            .HasOne<Course>(pd => pd.Course)
            .WithMany()
            .HasForeignKey(pd => pd.CourseId);
        modelBuilder.Entity<SubscriptionPurchaseDetails>()
            .HasOne<Subscription>(pd => pd.Subscription)
            .WithMany()
            .HasForeignKey(pd => pd.SubscriptionId);

        modelBuilder.Entity<Course>()
            .HasOne<User>(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.AuthorId);

        modelBuilder.Entity<User>()
            .HasOne<Subscription>()
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SubscriptionId);
    }
}
