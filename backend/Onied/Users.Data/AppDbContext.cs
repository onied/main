using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Users.Data.Entities;

namespace Users.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "b7e95e9c-19fb-4b3d-81a4-ccb77379f4c6",
            Name = "Admin",
            NormalizedName = "ADMIN".ToUpper(),
            ConcurrencyStamp = "eb9e8982-218e-47f8-8d4f-75055b023f98"
        }, new IdentityRole
        {
            Id = "81ad64cf-b580-4b51-aa34-4e74dbd44c4b",
            Name = "Student",
            NormalizedName = "STUDENT".ToUpper(),
            ConcurrencyStamp = "d27d2713-cc11-4f31-91ef-8dd2dfa3a303"
        });
    }
}
