using Courses.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.Courses.Helpers;

public class AppDbContextTest
{
    public static AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new AppDbContext(options.Options);
    }
}
