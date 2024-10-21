using Microsoft.EntityFrameworkCore;
using Support.Data.Models;

namespace Support.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<SupportUser> SupportUsers { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Chat>().HasMany<Message>(chat => chat.Messages).WithOne(message => message.Chat);
        modelBuilder.Entity<Chat>().HasOne<SupportUser>(chat => chat.Support).WithMany(user => user.ActiveChats);
        modelBuilder.Entity<SupportUser>().HasAlternateKey(user => user.Number);
        modelBuilder.Entity<SupportUser>().Property(user => user.Number).ValueGeneratedOnAdd();
    }
}
