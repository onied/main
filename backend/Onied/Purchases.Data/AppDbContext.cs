using Microsoft.EntityFrameworkCore;
using Purchases.Data.Enums;
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
            .HasForeignKey<PurchaseDetails>(pd => pd.Id);

        modelBuilder.Entity<PurchaseDetails>()
            .HasDiscriminator(pd => pd.PurchaseType)
            .HasValue<PurchaseDetails>(PurchaseType.Any)
            .HasValue<CoursePurchaseDetails>(PurchaseType.Course)
            .HasValue<CertificatePurchaseDetails>(PurchaseType.Certificate)
            .HasValue<SubscriptionPurchaseDetails>(PurchaseType.Subscription);

        modelBuilder.Entity<CoursePurchaseDetails>()
            .Property(pd => pd.CourseId)
            .HasColumnName("CourseId");
        modelBuilder.Entity<CoursePurchaseDetails>()
            .HasOne<Course>(pd => pd.Course)
            .WithMany()
            .HasForeignKey(pd => pd.CourseId);

        modelBuilder.Entity<CertificatePurchaseDetails>()
            .Property(pd => pd.CourseId)
            .HasColumnName("CourseId");
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
            .HasOne<Subscription>(u => u.Subscription)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SubscriptionId);

        var freeSubscription = new Subscription()
        {
            Id = 1,
            Title = "Микрочелик",
            Price = 0,
            ActiveCoursesNumber = 0,
            CoursesHighlightingEnabled = false,
            CertificatesEnabled = false,
            AdsEnabled = false,
        };
        modelBuilder.Entity<Subscription>().HasData(freeSubscription);
        modelBuilder.Entity<Subscription>().HasData(new Subscription()
        {
            Id = 2,
            Title = "Я карлик",
            Price = 2_000,
            ActiveCoursesNumber = 3,
            CoursesHighlightingEnabled = false,
            CertificatesEnabled = false,
            AdsEnabled = false,
        });
        modelBuilder.Entity<Subscription>().HasData(new Subscription()
        {
            Id = 3,
            Title = "Король инфоцыган",
            Price = 10_000,
            ActiveCoursesNumber = -1,
            CoursesHighlightingEnabled = true,
            CertificatesEnabled = true,
            AdsEnabled = true,
        });

        var firstCourseAuthorId = Guid.Parse("e768e60f-fa76-46d9-a936-4dd5ecbbf326");
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = firstCourseAuthorId,
            SubscriptionId = freeSubscription.Id,
        });

        modelBuilder.Entity<Course>().HasData(new Course
        {
            Id = 1,
            AuthorId = firstCourseAuthorId,
            Title = "Название курса. Как я встретил вашу маму. Осуждаю.",
            HasCertificates = true
        });
    }
}
