using Microsoft.EntityFrameworkCore;
using Notifications.Data.Models;

namespace Notifications.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications { get; set; } = null!;
}
