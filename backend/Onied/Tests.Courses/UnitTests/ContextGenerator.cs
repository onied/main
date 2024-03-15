using Courses;
using Microsoft.EntityFrameworkCore;

namespace Tests.Courses.UnitTests;

public class ContextGenerator
{
    public static AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new AppDbContext(options.Options);
    }
}